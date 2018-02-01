#
# New-MigrationFile.psm1
#
# Contains all functionality needed to create migration files to your project

# Migration table template

$createMigrationTableTemplate = 'namespace $namespace$.Migrations
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;

    using Redshift.Orm.Database;

    /// <summary>
    /// Creates migration table.
    /// </summary>
    internal class $name$ : MigrationBase
    {
        /// <summary>
        /// Gets the unique <see cref="Guid"/> of the <see cref="IMigration"/>. This must be generated pre-compile time.
        /// </summary>
        public override Guid Uuid 
        {
            get 
            { 
                return Guid.Parse("$guid$"); 
            }
        }

        /// <summary>
        /// Gets the name of the <see cref="IMigration"/>.
        /// </summary>
        public override string Name
        {
            get 
            { 
                return this.GetType().Name; 
            }
        }

        /// <summary>
        /// Gets the full name of the migration. The full name of a migration is the <see cref="IMigration.Name"/> property
        /// prepended with a long date. This is done for sorting purposes.
        /// </summary>
        public override string FullName
        {
            get
            {
                return string.Format("{0}_{1}", "$date$", this.Name);
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public override string Description 
        {
            get 
            { 
                return "Creates the Migration table so that the application can write migration patch logs."; 
            }
        }

        /// <summary>
        /// Gets the version this migration belongs to
        /// </summary>
        public override Version Version
        {
            get
            {
                return new Version($version$);
            }
        }

        /// <summary>
        /// The method that executes to apply a migration.
        /// </summary>
        public override void Migrate()
        {
            var migrationTableTemplate = new MigrationRecord();
            DatabaseSession.Instance.Connector.CreateTable(migrationTableTemplate);

            foreach (var property in migrationTableTemplate.GetType().GetProperties().Where(p => !CustomAttributeExtensions.IsDefined(p, typeof(IgnoreDataMemberAttribute))).ToList())
            {
                DatabaseSession.Instance.Connector.CreateColumn(property, migrationTableTemplate);
            }

            DatabaseSession.Instance.Connector.CreatePrimaryKeyConstraint(migrationTableTemplate);
        }

        /// <summary>
        /// The method that executes if a migration fails.
        /// </summary>
        /// <remarks>To be used in case of migration failure. If the migration is fully transactioned, then this can be overriden and left blank. If it is not, then you can safely run this.Reverse().</remarks>
        public override void MigrationReset()
        {

        }

        /// <summary>
        /// The method that executes if a migration needs to be rolled back.
        /// </summary>
        public override void Reverse()
        {
            var migrationTableTemplate = new MigrationRecord();
            DatabaseSession.Instance.Connector.DeleteTable(migrationTableTemplate);
        }

        /// <summary>
        /// Deletes the record from the migration table.
        /// </summary>
        public override void Delete()
        {
            // nothing needed
        }

        /// <summary>
        /// Checks whether the migration should execute.
        /// </summary>
        /// <returns>True if the migration should run.</returns>
        public override bool ShouldMigrate()
        {
            var migrationTableTemplate = new MigrationRecord();
            return !DatabaseSession.Instance.Connector.CheckTableExists(migrationTableTemplate);
        }
    }
}'

# Blank migration template

$migrationTemplate = 'namespace $namespace$.Migrations
{
    using System;

    using Redshift.Orm.Database;

    /// <summary>
    /// The purpose of the <see cref="$name$"/> migration is to ....
    /// </summary>
    internal class $name$ : MigrationBase
    {
        /// <summary>
        /// Gets the unique <see cref="Guid"/> of the <see cref="IMigration"/>. This must be generated pre-compile time.
        /// </summary>
        public override Guid Uuid 
        {
            get 
            { 
                return Guid.Parse("$guid$"); 
            }
        }

        /// <summary>
        /// Gets the name of the <see cref="IMigration"/>.
        /// </summary>
        public override string Name
        {
            get 
            { 
                return this.GetType().Name; 
            }
        }

        /// <summary>
        /// Gets the full name of the migration. The full name of a migration is the <see cref="IMigration.Name"/> property
        /// prepended with a long date. This is done for sorting purposes.
        /// </summary>
        public override string FullName
        {
            get
            {
                return string.Format("{0}_{1}", "$date$", this.Name);
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public override string Description
        {
            get 
            { 
                return "Some description here."; 
            }
        }

        /// <summary>
        /// Gets the version this migration belongs to
        /// </summary>
        public override Version Version
        {
            get
            {
                return new Version($version$);
            }
        }

        /// <summary>
        /// The method that executes to apply a migration.
        /// </summary>
        public override void Migrate()
        {
            // what to do to migrate
        }

        /// <summary>
        /// The method that executes if a migration fails.
        /// </summary>
        /// <remarks>To be used in case of migration failure. If the migration is fully transactioned, then this can be overriden and left blank. If it is not, then you can safely run this.Reverse().</remarks>
        public override void MigrationReset()
        {
            // what to do in case of migration fail
        }

        /// <summary>
        /// The method that executes if a migration needs to be rolled back.
        /// </summary>
        public override void Reverse()
        {
            // what to do to roll back the migration
        }

        /// <summary>
        /// The seeds the database if needed. This method can be left empty.
        /// </summary>
        public override void Seed()
        {
            // any required seeding
#if DEBUG

#endif
        }

        /// <summary>
        /// Checks whether the migration should execute.
        /// </summary>
        /// <returns>True if the migration should run.</returns>
        public override bool ShouldMigrate()
        {
            // migration condition
            return base.ShouldMigrate();
        }
    }
}'

# Creates a Migration file and puts it in the Migrations folder. If this folder does not exist
# creates the folder and adds the CreateMigrationTable migration in it when prompted.
# param $name - the name of the migration (mandatory not empty)
# param $version - the version that this migration is intended for (optional, default 1.0.0)
function New-MigrationFile {

	#Cmdlet inputs and validation
	[CmdletBinding()]
    param (
		[Parameter(Mandatory=$true,
					HelpMessage="The name of the migration to be created.")]
        [ValidateNotNullOrEmpty()]
		[alias("n")]
        [string]$name,
		[Parameter(Mandatory=$false,ValueFromPipeline=$true,
					HelpMessage="The version that this migration is targeted for. Must be in two or 3 digit dot format: 1.0.3. Default: 1.0.0")]
        [ValidatePattern('\d+(\.\d+)+')]
		[alias("v")]
        [string]$version = '1.0.0'
    )

	# Convert version to consumable
	$versionLiteral = $version.Replace('.',', ')
    
	Write-Verbose "The version converted to $versionLiteral"

	# Get all the needed path information. Gets the default Project. Make sure this is correct at the top of the
	# Package Manager Console.
	# If start-up project changes mid-session, this is not taken into account until IDE restarts.
	$project = Get-Project

	# Construct the formatted date for filename and insides. 
	$date = Get-Date -Format "yyyyMMddHHmmss"

	Write-Verbose "The name is $name"

	Write-Verbose "The date is $date"
	
	# Get project path	
	$projectPath = Split-Path $project.FileName
	
	Write-Verbose "Project path: $projectPath"

	# Get project default namespace
	$namespace = $project.Properties.Item("RootNamespace").Value

	Write-Verbose "Namesaspace: $namespace"

	$migrationsFolderPath = Join-Path -Path $projectPath -ChildPath Migrations

	Write-Verbose "Migration Folder Path: $migrationsFolderPath"
	
	# Get the migrations folder, need a check here if exists

	Try{
		$migrationFolder = Get-Item -Path $migrationsFolderPath -erroraction stop
		$firstTime = $false
	}Catch [System.Exception]{

		# If folder did not exist we go ahead with the setup
		Write-Verbose "Migration folder not found in project."
		Write-Host "First time migration command run detected. Setting up..."
		
		# Add It to the proect
		$migrationFolder = New-Item -Path $migrationsFolderPath -ItemType Directory -Force
        
		# Set the trigger for the migration table creation
		$firstTime = $true
	}
    
	# Setup procedure
    if($firstTime)
    {
		Write-Verbose "Writing migration file for migration table..."

		# Write the migration file
        Write-MigrationFile $date 'CreateMigrationTable' '0, 0, 0' $namespace $migrationsFolderPath $createMigrationTableTemplate

		Write-Verbose "Done"

		# Sleep for one second to ensure timestamp is different
        Start-Sleep -Seconds 1

        # reset date so that the new migration is older
        $date = Get-Date -Format "yyyyMMddHHmmss"
    }
    
	Write-Verbose "Writing migration file..."

    # Write the original migration  		
	Write-MigrationFile $date $name $versionLiteral $namespace $migrationsFolderPath $migrationTemplate
}

#
# Writes the migration to file
#

function Write-MigrationFile($date, $name, $version, $namespace, $migrationFolder, $template)
{
    Try
    {
		# Get the literal path to the migration folder
        $migrationFolderPath = $migrationFolder

		Write-Verbose "Migration folder path set $migrationFolderPath"

		# Set up version for insertion in the filename
        $versionForFile = $version.Replace(', ','_')

		# Construct filename
        $createMigrationTableFileName = "$($date)_$($versionForFile)_$($name).cs"

	    Write-Verbose "First migration file will write to file at $createMigrationTableFileName"

		# Construct full path to the new file
        $templateFilePath = Join-Path -Path $migrationFolderPath -ChildPath $createMigrationTableFileName

        Write-Verbose "Full path to new migration: $templateFilePath"

		# Write an empty file there
        "" | sc $templateFilePath

		# Add it to the project
	    $templateFile = New-Item -Path $templateFilePath -ItemType File -Force

		# Get the full path (just to make sure its correct)
	    # $templateFilePath = $templateFile.Properties.Item("FullPath").Value

	    Write-Verbose "Full path to new migration: $templateFilePath"

	    # replace relevant code
	    Format-MigrationFile $templateFilePath $template $date $version $name $namespace $createMigrationTableFileName

	    Write-Host "First migration file written to file at $createMigrationTableFileName"
    }
    Catch
    {
        Write-Warning "Failed to write $name"
    }
}

#
# Formats the new migration file with the correct 
#

function Format-MigrationFile($filePath, $template, $timestamp, $version, $name, $namespace, $filename){

	Try{

		Write-Verbose "Replacing tokens in $filePath"

        # generate guid
		$guid = [guid]::NewGuid()

        # replace namespace
		$template = $template.replace('$namespace$', $namespace) 
		
		$template = $template.replace('$name$', $name)
		
		$template = $template.replace('$date$', $timestamp)
		$template = $template.replace('$version$', $version)
		$template = $template.replace('$guid$', $guid)
		$template = $template.replace('$filename$', $filename)

		# Use Out-File here, seems to be more reliable than set-content. Has no problem reading the stream so far.

		$template | Out-File $filePath -Encoding UTF8
		        
		Write-Verbose "Done replacing tokens in $filePath"
	}
	Catch
	{
		Write-Warning "Failed to replace tokens in $filePath"
	}
}

Export-ModuleMember New-MigrationFile