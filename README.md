# Grassroots

## Overview
Grassroots is an app built with the Entity framework to help grassroots organizations manage resources for a campaign.  Users can create **Activities** (single-person actions), **Gatherings** (group actions), see data about their contributions on the **My Report** page, see data about the campaign on the **Campaign Report** page, and view a calendar with any Activities or Gatherings they scheduled on the **My Schedule** page.  

## Video Walkthrough
https://www.youtube.com/watch?v=8ITCrvrkRXQ&feature=emb_title

## Installation
1.  Install Microsoft SQL Server and Visual Studio Community
2.  Clone down the repo
3.  Start grassroots.sln
4.  Open Package Manager Console in Visual Studio
5.  In Package Manager enter the command **Add-Migration SeedDatabase**
6.  In Package Manager enter the command **Update Database**
7.  Ctrl+F5 to run the app

## Walkthrough

### Homepage
<img src="https://user-images.githubusercontent.com/62182071/103713943-7fe9a880-4f8b-11eb-8452-11d75af2c19d.png">

### My Activites
<img src="https://user-images.githubusercontent.com/62182071/103714488-c390e200-4f8c-11eb-8e0a-64a31d3f3669.png">

The My Activities page displays single-person actions taken on behalf of a campaign.  Here the user can see:
* Every Activity the user has scheduled.
* The city and county for each activity.
* The start and finish time for each activity.

The page also features the ability to filter by county, sort by each column header, and to navigate through each page of activites (the app prints five activities per page).

### Gatherings
<img src="https://user-images.githubusercontent.com/62182071/103718861-bf1cf700-4f95-11eb-8390-b84ee3a5f098.png">

The Gatherings page displays group actions taken on behalf of a campaign.  Here the user can see:
* Every Gathering the user has scheduled.
* The maximum amount of people that can attend the gathering.
* The city and county for each activity.
* The start and finish time for each activity.
* Gatherings you have joined.
* Gatherings created by someone else that you can join.

The page also features the ability to filter by county, sort by each column header, and to navigate through each page of gatherings (the app prints five gatherings per page).

### My Report

<img src="https://user-images.githubusercontent.com/62182071/103726471-ff38a580-4fa6-11eb-9c91-0e2d343ee952.png">

The My Report page lets the user know how many hours they've completed and how many hours the average user has completed.  Hours are calculated after an activity or gathering has passed is finish time.  The page also features a complete list of the user's activities and gatherings.

### Campaign Report

<img src="https://user-images.githubusercontent.com/62182071/103728160-3446f700-4fab-11eb-9098-d1c14f5c0b2a.png">

The Campaign Report page lets the user know the top-five counties by population and the top-five counties in need of resources in the campaign.  It features a multi-variable pie chart representing the population and volunteer hours for each county.  It also features a traditional pie chart for the adjusted population of each county.  Adjust population is caluclated by taking population and subtracting the volunteer hours multiplied by five (Population - Hours * 5).

### My Schedule

<img src="https://user-images.githubusercontent.com/62182071/103732363-357d2180-4fb5-11eb-81bc-f0f5d3bc6669.png">

The My Schedule page features a calendar with all of the activities and gatherins the user has scheduled.  You can click on the link to each activity or gathering to view the details.
