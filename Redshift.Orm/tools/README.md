# Introduction

This file explains how to use the migration powershell module.

# Importing

To load the module into memory while VS is running, open the Package Manager Console and run the following command:

```
Import-Module -Name .\src\path\to\New-MigrationFile.psm1 -Verbose
```

This has to be done everytime that VS is opened.

# Usage

Open the Package Manager Console and run:

```
New-MigrationFile [-name] <string> [[-version] <string>] [-Verbose]
```  

Where the name string is a descriptive name of the description. The version string specifies the version that this migration is targeted for. Must be in two or 3 digit dot format: 1.0.3 Default: 1.0.0"

Specify `-Verbose` to get more output.

Sample:

```
New-MigrationFile "MyNewMigration" "2.3.4"
```

# Editing the Script

When editing the script you need to use Remove/re-Import method. If you have previously Imported the module then run:

```
Remove-Module New-MigrationFile
```

Then import it again:

```
Import-Module -Name .\Redshift.Orm\tools\New-MigrationFile.psm1 -Verbose
```
