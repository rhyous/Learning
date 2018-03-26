# Get working directory
cd "$PSScriptRoot"

# Start Name server
$LogServerName = "LogServer1"
$nsEndpoint="tcp://127.0.0.1:6001"
Start-Process .\NameServer\Rhyous.CS6210.Hw1.NameServer.exe -ArgumentList "e=$($nsEndpoint)","lsn=$($LogServerName)","ulh=true"

Start-Sleep -s 4

# Start log server
Start-Process .\LogServer\Rhyous.CS6210.Hw1.Logger.exe -ArgumentList "n=$($LogServerName)","ns=$($nsEndpoint)","c=true"

# Start OutBreak Analyzer for each disease
$diseases = "Influenza", "ChickenPox", "Measles"
$endpointTemplate="tcp://127.0.0.1:55"
$analyzers = @();
$publishers = "";
$port=0;
Foreach ($disease in $diseases)
{
    $endpoint = "$($endpointTemplate)5$($port)"
    $pubendpoint = "$($endpointTemplate)6$($port)"
	if ($publishers -ne "") {
		$publishers += ","
	}
	$publishers += $pubendpoint;
	$analyzers += $endpoint;
    Start-Process .\Analyzer\Rhyous.CS6210.Hw1.OutBreakAnalyzer.exe -ArgumentList "n=`"Outbreak Analyzer: $($disease)`"","d=$($disease)","e=$($endpoint)","p=$($pubendpoint)","ns=$($nsEndpoint)"
	$port++;
} 

For ($i=0; $i -lt $analyzers.Length; $i++) {
    # Start 1 Health Districts    
    $hdendpoint = "$($endpointTemplate)7$($i)"
    Start-Process .\HealthDistrict\Rhyous.CS6210.Hw1.HealthDistrict.exe -ArgumentList "n=`"Health District $($i)`"","d=$($hdendpoint)","p=$($publishers)","ns=$($nsEndpoint)"
   
    # Start 1 or 2 Simulators for every Health District
    $count = Get-Random -Minimum 1 -Maximum 2
    For ($j=0; $j -le $count; $j++) 
    {
        Start-Process .\Simulator\Rhyous.CS6210.Hw1.Simulator.exe -ArgumentList "n=`"Simulator $($i)`"","e=$($hdendpoint)","t=43200","u=30","ns=$($nsEndpoint)"
    }
}