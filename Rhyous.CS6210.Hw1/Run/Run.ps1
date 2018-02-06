# Get working directory
cd "$PSScriptRoot"


# Start log server
$endpointLogServer="tcp://127.0.0.1:5501"
./Logger/Rhyous.CS6210.Hw1.Logger.exe


# Start OutBreak Analyzer for each disease
$diseases = "Influenza", "ChickenPox", "Measles"
$i = 0;
$endpointTemplate="tcp://127.0.0.1:55"
Foreach ($disease in $diseases)
{
    $endpoint = "$endpointTemplate5$i"
    $pubendpoint = "$endpointTemplate6$i"
    ./Analyzer/Rhyous.CS6210.Hw1.OutBreakAnalyzer.exe n="Outbreak Analyzer: $disease" d=$disease a=$endpoint p=$pubendpoint

    # Start 1 Health Districts    
    $hdendpoint = "$endpointTemplate7$i"
    ./HealthDistrict/Rhyous.CS6210.Hw1.HealthDistrict.exe n="Health District $i" d=$endpoint p=$pubendpoint

    
    # Start 1 or 2 Simulators for every Health District
    $count = Get-Random -Minimum 1 -Maximum 2
    For ($j=0; $j -le $count; $j++) 
    {
        ./Simulator/Rhyous.CS6210.Hw1.Simulator.exe n="Simulator $i" e=$endpoint t=43200 u=30
    }    
    i++
}