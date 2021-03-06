# What Is This?

This is a minimalist ODBC configuration tool, which is meant for quickly
finding and editing DSNs when you have a lot of them.

![Exmaple Usage](images/example.gif)

The controls are probably what you'd expect. There are three ways you can summon the configuration window for a particular DSN:

- If you have more than one search result, double-click the one you want or select it and press enter.
- If you have only one search result, you can either:
    - Press enter inside of the search bar, or 
    - Double-click anywhere in the search results.

You can also enter the search listing by hitting the down key within the search
box, and navigate the search entries using your arrow keys.

# How Do I Build It?

You can either use the command line and run`msbuild odbc_config.sln`, or build
it through Visual Studio.

# Limitations

- Currently it depends upon the driver to do configuration, which uses an API
  only available on Windows. I find odbc.ini comfortable to read and edit using
  normal text editors anyway, so this isn't likely to change any time soon.

- DSNs cannot currently be created or removed, only edited. I may add these
  features at a later date (pull requests also welcome).
