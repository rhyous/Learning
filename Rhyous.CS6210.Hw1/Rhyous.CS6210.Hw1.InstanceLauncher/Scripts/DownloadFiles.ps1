$Logfile = "C:\LaunchScript.log"

Function LogWrite
{
   Param ([string]$logstring)

   Add-content $Logfile -value $logstring
}

LogWrite("Starting script")
Import-Module "C:\Program Files (x86)\AWS Tools\PowerShell\AWSPowerShell\AWSPowerShell.psd1"
LogWrite("Imported AWSPowershell.psd1")

# Your account access key - must have read access to your S3 Bucket
$accessKey = "{AccessKey}"
LogWrite($accessKey)
# Your account secret access key
$secretKey = "{SecretKey}"
LogWrite($secretKey)
# The region associated with your bucket e.g. eu-west-1, us-east-1 etc. (see http://docs.aws.amazon.com/AWSEC2/latest/UserGuide/using-regions-availability-zones.html#concepts-regions)
$region = "{region}"
LogWrite($region)
# The name of your S3 Bucket
$bucket = "{bucket}"
LogWrite($bucket)
# The folder in your bucket to copy, including trailing slash. Leave blank to copy the entire bucket
$keyPrefix = "{RemoteDirectory}"
LogWrite($keyPrefix)
# The local file path where files should be copied
$localPath = "{InstanceDirectory}"
LogWrite($localPath)

LogWrite("Starting file copy")
$objects = Get-S3Object -BucketName $bucket -KeyPrefix $keyPrefix -AccessKey $accessKey -SecretKey $secretKey -Region $region
LogWrite($objects)

foreach($object in $objects) {
    $localFileName = $object.Key -replace $keyPrefix, ''
    if ($localFileName -ne '') {
        $localFilePath = Join-Path $localPath $localFileName
        Copy-S3Object -BucketName $bucket -Key $object.Key -LocalFile $localFilePath -AccessKey $accessKey -SecretKey $secretKey -Region $region
		LogWrite("Copied file: $($localFilePath)")
    }
}

LogWrite("File copy complete")


LogWrite("Running script")
c:\Run\Run.ps1
LogWrite("Finished running script")

Write-S3Object -BucketName $bucket -File $Logfile -Key LaunchScript.log
