using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace odbc_config
{
    public partial class SearchForm : Form
    {
        private static readonly string SEARCH_PLACEHOLDER = "Search...";

        private DSNOperations DSNOps;
        private List<DSNInfo> FoundDSNs;

        public SearchForm()
        {
            DSNOps = new DSNOperations();
            FoundDSNs = new List<DSNInfo>();

            InitializeComponent();
            Focus();
        }

        private void searchText_Enter(object sender, EventArgs e)
        {
            if (SearchText.Text == SEARCH_PLACEHOLDER)
            {
                SearchText.Text = "";
            }
        }

        private void searchText_Leave(object sender, EventArgs e)
        {
            if (SearchText.Text == "")
            {
                SearchText.Text = SEARCH_PLACEHOLDER;
            }
        }

        private void UpdateDSNList()
        {
            var pattern = SearchText.Text;
            if (pattern == SEARCH_PLACEHOLDER || pattern.Trim() == "")
            {
                pattern = null;
            }

            SearchResults.BeginUpdate();

            SearchResults.Items.Clear();
            FoundDSNs.Clear();
            foreach (var dsn in DSNOps.SearchDSN(pattern))
            {
                var display = string.Format(
                    "{0} ({1})",
                    dsn.DSN,
                    dsn.Type == DSNType.System ? "System" : "User"
                );

                SearchResults.Items.Add(display);
                FoundDSNs.Add(dsn);
            }

            SearchResults.EndUpdate();
        }

        private void ConfigureDSN(DSNInfo info)
        {

            DSNOps.ConfigureDSN(info, Handle);

            // Some drivers have the ability to rename their DSNs, which would
            // invalidate our results unless we did this
            UpdateDSNList();
        }

        private void searchResults_DoubleClick(object sender, EventArgs e)
        {
            DSNInfo? selection = null;
            for (var i = 0; i < FoundDSNs.Count; i++)
            {
                if (SearchResults.GetSelected(i))
                {
                    selection = FoundDSNs[i];
                    break;
                }
            }

            if (!selection.HasValue)
            {
                if (FoundDSNs.Count != 1)
                {
                    return;
                }
                else
                {
                    selection = FoundDSNs[0];
                }
            }

            ConfigureDSN(selection.Value);
        }

        private void searchText_TextChanged(object sender, EventArgs e)
        {
            UpdateDSNList();
        }

        private void SearchText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && FoundDSNs.Count == 1)
            {
                ConfigureDSN(FoundDSNs[0]);
            }
        }
    }
}
