%% this to be run when the game starts.
%% update problem data edgelist.
%% run the generate contractor 

%% update problem data edgelist
%% run main
clc
clear
X = importdata('C:/Users/Uttkarsh/Desktop/Debris_work_folder/work_debris/Frontend/Debris/Assets/Database/Output/edgelist_forMatlab.csv');

load('ProblemData(Instance2).mat');

header = X(1,:);
X(1,:) = [];

EdgeList = X;

GenerateContractor(); 