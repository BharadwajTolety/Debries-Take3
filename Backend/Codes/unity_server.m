%unity connection - matlab server

clc
clear all
tcpipServer = tcpip('0.0.0.0',55002,'NetworkRole','Server');
fprintf('matlab is listening for unity now...');
while(1)
data = membrane(1);
fopen(tcpipServer);
rawData = fread(tcpipServer,24,'char');
for i=1:24 rawwData(i)= char(rawData(i));
    
if strcmp(rawwData, 'matlab can read CSV now!')
    fprintf('start reading csv now!');
    unity_start();
end
end
fclose(tcpipServer);
end