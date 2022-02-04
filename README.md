# Loan Broker Minimal API

A version of the Loan Broker example from Enterprise Integration Patterns using .NET Minimal APIs, Azure Service Bus and Azure Table Storage.

Adapted from Gregor Hohpe's AWS implementation found at https://www.enterpriseintegrationpatterns.com/ramblings/loanbroker_stepfunctions.html

The overall workflow of the broker application is controlled by an ASP.NET Mininmal API with a background worker. The broker is responsible for obtaining a credit score from a separate 'Credit Bureau' service (implemented as an ASP.NET Minimal API) and requesting loan quotations from multiple 'Bank' services.

Loan quotations are requested via a Service Bus Topic which the various bank services subscribe to.

Each bank service then provides a quote to the broker via a Service Bus Queue. The background worker in the broker collects quotations, storing the quotations in Azure Table Storage. Once the broker has received the appropriate number of quotes the results are emailed to the requestor using SendGrid.

Each service is Docker enabled to allow deployment to one of Azure's container hosting options including the new Azure Container Apps.
