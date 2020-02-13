+++
title = "Overview"
chapter = false
weight = 1
+++

## Goal ##

You already have deployed the sample application to Azure manually. Today we want to dive into Azure DevOps to show you how you can automate your build and deployment process.
In addition we want to show you how you can plan and manage your work with Azure Boards and how you collaborate on code development using Azure Repo.

## Azure Boards

![Azure Boards](../img/boards.svg)

We use Azure Boards to plan your work for Day4. During the day you will use Azure Boards to define Features, UserStories and Tasks to reflect the progress of Day4.
At the end of the day you will know how to plan and track your work with Azure Boards and how you can plan your agile iterations.

![Goal Azure Boards](../img/goal-azure-boards.png)

## Azure Repos

![Azure Repo](../img/repos.svg)

We use Azure Repo to work with a Git repository. During the day you will work with Git branches, commits and PullRequests.

## Azure Pipelines

![Azure Pipelines](../img/pipelines.svg)

We use Azure Pipelines to build and deploy the sample application to a Development and Testing stage on your Azure subscription.
At the end of the day you will know how to create a CI/CD Pipeline for all Microservices of the sample application and how you continuously and consistently deploy services to Azure during your application lifcycle process.

To give you a short overview of all Microservices that are part of the sample application the following table shows you all services and the runtime that is used to implement the service.

|Service| Tech|
|-------|-----|
|SCM Contacts API|ASP.NET Core|
|SCM Resource API|ASP.NET Core|
|SCM Search|ASP.NET Core|
|SCM Visitreports API|NodeJs|
|SCM Textanalytics|NodeJs|
|SCM Frontend|NodeJs|


