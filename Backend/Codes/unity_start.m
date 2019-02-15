%% this to be run when the game starts.
%% update problem data edgelist.
%% run the generate contractor 

%% update problem data edgelist
%% run main
clc
clearvars -except tcpipServer
X = importdata('C:/Users/Uttkarsh/Desktop/Debris_work_folder/work_debris/Frontend/Debris/Assets/Database/Output/edgelist_forMatlab.csv');

load('brushed_edges.mat');
load('ProblemData(Instance2).mat');

header = X(1,:);
X(1,:) = [];
B = [];

for k = 1:size(X)
    B = str2double(regexp(num2str(X(k,3)),'\d','match'));
    brushed_edges{k,1} = X(k,1);
    brushed_edges{k,2} = X(k,2);
    brushed_edges{k,3} = B;
end

EdgeList = X;
GenerateContractor(); 
