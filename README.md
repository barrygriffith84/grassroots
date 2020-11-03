# Grassroots

## Overview
Grassroots is an app built with the Entity framework to help grassroots organizations manage resources for a campaign.  Users can create **Activities** (single-person actions), **Gatherings** (group actions), see data about their contributions on the **My Report** page, see data about the campaign on the **Campaign Report** page, and view a calendar with any Activities or Gatherings they scheduled on the **My Schedule** page.  

## Installation
1.  Install Microsoft SQL Server and Visual Studio Community
2.  Clone down the repo
3.  Start grassroots.sln
4.  Open Package Manager Console in Visual Studio
5.  In Package Manager enter the command **Add-Migration SeedDatabase**
6.  In Package Manager enter the command **Update Database**
7.  Ctrl+F5 to run the app