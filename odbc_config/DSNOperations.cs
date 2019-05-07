using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace odbc_config
{
    enum DSNType
    {
        System = 1,
        User = 2
    }

    /// <summary>
    /// Contains the basic information about a DSN and its driver.
    /// </summary>
    struct DSNInfo
    {
        public DSNType Type;
        public string Driver;
        public string DSN;

        public DSNInfo(DSNType type, string driver, string dsn)
        {
            Type = type;
            Driver = driver;
            DSN = dsn;
        }
    }

    class DSNOperations
    {
        private static readonly string ODBC_INI_PATH = "SOFTWARE\\ODBC\\ODBC.INI\\";
        private static readonly short ODBC_CONFIG_DSN = 2;
        private static readonly short ODBC_CONFIG_SYS_DSN = 5;

        [DllImport("odbccp32.dll")]
        private static extern bool SQLConfigDataSource(IntPtr window, short request, string driver, string attributes);

        /// <summary>
        /// Traverses the registry, starting from the given key, until the
        /// subkey at the given path is found.
        /// </summary>
        /// <remarks>
        /// The path should be a relative path of the form
        /// "SOFTWARE\ODBC\ODBC.INI\ODBC Data Sources".
        /// </remarks>
        private RegistryKey OpenSubKeyAtPath(RegistryKey key, string path)
        {
            var isBaseKey = true;
            foreach (var component in path.Split('\\'))
            {
                var subkey = key.OpenSubKey(component, RegistryKeyPermissionCheck.ReadSubTree);
                var keyName = key.Name;
                if (!isBaseKey)
                {
                    key.Close();
                }

                isBaseKey = false;
                key = subkey ?? throw new ArgumentException("Could not find subkey " + component + " on key " + keyName);
            }

            return key;
        }

        /// <summary>
        /// Returns all the DSNs which match the given search string using fuzzy matching.
        /// </summary>
        public IEnumerable<DSNInfo> SearchDSN(string searchString)
        {
            var dsnInfos = new List<Tuple<int, DSNInfo>>();

            using (var dataSources = OpenSubKeyAtPath(Registry.LocalMachine, ODBC_INI_PATH + "ODBC Data Sources"))
            {
                foreach (var dsnName in dataSources.GetValueNames())
                {
                    // The default key is usually unassigned, but doesn't
                    // contain any useful information if it is
                    if (dsnName == "") continue;

                    var info = new DSNInfo(DSNType.System, dataSources.GetValue(dsnName) as string, dsnName);
                    if (searchString == null)
                    {
                        dsnInfos.Add(Tuple.Create(0, info));
                    }
                    else
                    {
                        var score = FuzzyMatcher.FuzzyMatch(dsnName, searchString);
                        if (score.HasValue)
                        {
                            dsnInfos.Add(Tuple.Create(score.Value, info));
                        }
                    }
                }
            }

            using (var dataSources = OpenSubKeyAtPath(Registry.CurrentUser, ODBC_INI_PATH + "ODBC Data Sources"))
            {
                foreach (var dsnName in dataSources.GetValueNames())
                {
                    if (dsnName == "") continue;
                    var info = new DSNInfo(DSNType.User, dataSources.GetValue(dsnName) as string, dsnName);

                    if (searchString == null)
                    {
                        dsnInfos.Add(Tuple.Create(0, info));
                    }
                    else
                    {
                        var score = FuzzyMatcher.FuzzyMatch(dsnName, searchString);
                        if (score.HasValue)
                        {
                            dsnInfos.Add(Tuple.Create(score.Value, info));
                        }
                    }
                }
            }

            return dsnInfos
                .OrderBy(a => a.Item1)
                .Select(a => a.Item2);
        }

        /// <summary>
        /// Calls the driver's setup routine to configure the DSN.
        /// </summary>
        public void ConfigureDSN(DSNInfo info, IntPtr window)
        {
            SQLConfigDataSource(
                window,
                info.Type == DSNType.System ? ODBC_CONFIG_SYS_DSN : ODBC_CONFIG_DSN,
                info.Driver,
                "DSN=" + info.DSN + "\0");
        }
    }
}
