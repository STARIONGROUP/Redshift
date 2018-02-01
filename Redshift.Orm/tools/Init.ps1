#
# init.ps1
#

param($installPath, $toolsPath, $package, $project)

Import-Module (Join-Path $toolsPath New-MigrationFile.psm1)