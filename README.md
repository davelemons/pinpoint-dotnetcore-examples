# pinpoint-dotnetcore-examples
Several (very rough) examples of calling the Amazon pinpoint API to create Endpoints, Events, and Segments

## Prerequisites 
- [Getting Started](https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/quick-start-s3-1-cross.html)
- `dotnet add package AWSSDK.Pinpoint`
- Modify `applicationId` variable.  Application ID (also called ProjectId) can be found in the Pinoint AWS Console
- `dotnet build`
- `dotnet run`

## Current API Calls
- UpdateEndpointsBatch
- GetEndpoint
- WriteEvents
- CreateSegment
- UpdateSegment
- GetSegment

The examples above should provide a good introduction and allow you to implement any of the other methods here: https://docs.aws.amazon.com/sdkfornet/v3/apidocs/items/Pinpoint/TPinpointClient.html