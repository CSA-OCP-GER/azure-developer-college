+++
title = "Overview"
chapter = false
weight = 1
+++

## Goal ##

To give you a little bit more context on what we will be building, here's the description of our sample application and the resulting architecture for *Day 2*.

## Application ##

The sample application we will be using to get to know all the Azure services throughout the workshop, will be a **Simple Contacts Management** (SCM) application. You can - surprisingly - create, read, update and delete contacts with it. Currently, we will be storing the contacts in an in-memory database. On *Day 3* we will learn about the various database services of Azure and add proper persistance to our services. 

Later that day, we will also add a second service to be able to add contact images - which will be stored in an *Azure Storage Account* (Blob). An *Azure Function* which will automatically be trigger through an *Azure Storage Queue* will be responsible to create thumbnails of the images in background.

The frontend for the application is a small, responsive Single Page Application written in *VueJS* (which is one of the popular frameworks at the moment). We will be using the cheapest option to host a static website like that: Azure Blob storage.

![day2_1](../img/day2_goal1.png "day2_1")
![day2_2](../img/day2_goal2.png "day2_2")
![day2_3](../img/day2_goal3.png "day2_3")

## Architecture ##

At the end of the day, we will have the following architecture up and running in your own Azure subscription.

![architecture](../img/architecture_day2.png "architecture")
