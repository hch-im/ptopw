1. Description
pTopW, the Windows version of pTop, is developed under .net framework 4. To run this program you operating system must be Windows 7 or a later version
because many system events that we used to build the energy model are only supported by these new platforms. Windows XP is not supported.

pTopW relies on system events to estimate the energy consumption of each device and each process. The energy models we build include several fundmental
parameters, which includes the power of the devices in each state. The user could modify these parameters based on their own platform to get more accur
-ate result. These parameters are defined in the configuration file "ptopw.conf". Through this file, the user could also setup the parameters, such as 
the sampling period, of the program.
2. Configuration file.
All the configuration information of pTopW is saved in the file "ptopw.conf". The use should change the parameters that we used to build the energy model
if you want to get more accurate result. 
a) CPU energy model
The CPU energy model requires three parameters:
ALPHA         0.6
MaxCPUPower   24.5  #Watts, the maximum power of CPU
MinCPUPower    3.8    #Watts, the minimum power of CPU
The maximum CPU power is the average power we required when we ran several several benchmarks that stress the processor. Through setup alpha the user could
fine tune the accuracy. 

b) Memory energy model
The memory energy model requires four parameters:
MemReadPower  2.016   #Watts, the power of memory during read operations
MemWritePower  2.016   #Watts, the power of memory may be different during read and write operations
MemIdlePower  1.20   #Watts, the power of memory may be different during read and write operations
MemReadSpeed  1015   #MB/s
MemWriteSpeed 1332
MemCopySpeed  1440
The first three parameters are the power of memory when it is in read, write and idle state. The last three parameters shows the speed of the memory when 
the memory is process different operations. The user could get these three parameters by running a software called MaxxMEM.

c) Disk energy model
The disk energy model requires three parameters:
DiskReadPower  2.78     #Watts, the power of disk during read operations
DiskWritePower  2.19     #Watts, the power of disk during write operations
DiskIdlePower  1.43      #Watts, the power of the disk when it is idle
These three parameters are required when the disk is processing different operations. 

d) The energy model of wireless network card
The energy model of wireless network card requires four parameters:
WirelessTransmitPower 2.268    #Watts, the power of wireless network card when transmit signal
WirelessReceivePower   2.289   #Watts, the power of wireless network card when receive signal
WirelessNICIdlePower   0.4249  #Watts, the idle power of wireless network card
WNICIndex 0                    #the index of wireless network interface

The parameter WNICIndex is the index of wireless network card in the iptables. The user can also select the wireless network card after run pTopW.
3. Build the source code
If the user want to build the source code, they must copy two files, "MyDll.dll" and "ptopw.conf" to the output folder of the ptow subproject. pTopW
used the functions supplied "MyDll.dll" to read iptables.

4. User Interfaces
pTopW supplied two kinds of user interfaces, performance counter and APIs. We write the dynamic energy information into performance counters. The user
could read the performance counters that under the category "wsu_ptopw", which defines four counters that related with the power of each component and
the total power. In each of these five performance counters, there are a group of instances that shows the power of each process. The name of the inst
-ance follows the format: {process id}_{process name}.

Except the performance counter, the user could also acquire the energy information through the functions that we defined in the subproject ptopwdll.
The user need to build this subproject and copy the output file ptopwdll.dll into their working directory.

5. Log
If the user only want to use the energy information to do some analyzation, they can also setup the configuration to make pTopW print the energy and
power information. The following is the three parameters:
DataRecordMode 1  # 1. Only save the final energy result.  2. Also save intermediate result.
SaveResult  0   # 1:save the collected data into files , 0 : do not save the result info files
FileName    ResultLog.txt    #file name that save the result, only useful when SaveResult = 1

Thanks for your interest to our work, if you have any problem during the use of pTop you can mail to ptop.mist@gmail.com! 