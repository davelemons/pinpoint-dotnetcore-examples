﻿using System;
using Amazon.Pinpoint; //dotnet add package AWSSDK.Pinpoint
using Amazon.Pinpoint.Model;
using System.Collections.Generic;

namespace pinpoint_samples
{
    class Program
    {
        static void Main(string[] args)
        {
            //See https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/quick-start-s3-1-cross.html#s3-1-cross-setup
            //for initial credential setup.  DO NOT include credentials in source code or project files!!!
            AmazonPinpointClient client = new AmazonPinpointClient();

            var applicationId = "1aa20d5ade5c4699a5df45ddad370a10";

            //UpsertEndpoint(s)
            var endpointAttributes = new Dictionary<string, List<string>>()
            {
              { "Groups", new List<string> { "GroupA", "GroupD" } },
              { "FavoriteColor", new List<string> { "Green" } },
            };

            var userAttributes = new Dictionary<string, List<string>>()
            {
              { "FirstName", new List<string> { "Test" } },
              { "LastName", new List<string> { "User" } },
            };

            EndpointUser user = new EndpointUser{
              UserId = "test@example.com",
              UserAttributes = userAttributes
            };

            EndpointBatchItem ep = new EndpointBatchItem {
              Address = "test@example.com",
              Id = "test@example.com",
              Attributes = endpointAttributes,
              ChannelType = "EMAIL",
              User = user
            };

            EndpointBatchRequest batchRequest = new EndpointBatchRequest{
              Item = new List<EndpointBatchItem>{ep} //Include other endpoints to update more than one...100 max
            };

            UpdateEndpointsBatchRequest updateEndpointBatchRequest = new UpdateEndpointsBatchRequest{
              ApplicationId = applicationId,
              EndpointBatchRequest = batchRequest
            };

            Console.WriteLine("Updating Endpoint...");
            UpdateEndpointsBatchResponse updateEndpointBatchResponse = client.UpdateEndpointsBatchAsync(updateEndpointBatchRequest).Result;
            Console.WriteLine("Results: {0} - {1} - {2}", updateEndpointBatchResponse.HttpStatusCode, updateEndpointBatchResponse.MessageBody.Message, string.Join(Environment.NewLine, updateEndpointBatchResponse.ResponseMetadata.Metadata));

            //GetEndpoint
            GetEndpointRequest request = new GetEndpointRequest{
              ApplicationId = applicationId,
              EndpointId = "test@example.com"
            };
            
            Console.WriteLine("Fetching Endpoint...");
            GetEndpointResponse response = client.GetEndpointAsync(request).Result;
            Console.WriteLine("Found: {0}", response.EndpointResponse.Address);

            //Build Segment
            AttributeDimension ad = new AttributeDimension{
              AttributeType = AttributeType.INCLUSIVE,
              Values = new List<string>{"GroupC"}
            };

            var attributes = new Dictionary<string, AttributeDimension>()
            {
              { "Groups", ad }
            };
            SegmentDimensions sd = new SegmentDimensions{
              Attributes = attributes
            };

            SegmentGroup sg = new SegmentGroup{
              SourceType = "ANY",
              Type = "ALL",
              Dimensions = new List<SegmentDimensions>{sd}
            };

            SegmentGroupList sgl = new SegmentGroupList{
              Include = "ALL",
              Groups = new List<SegmentGroup>{sg}
            };

            WriteSegmentRequest segment = new WriteSegmentRequest{
              Name = "GroupC",
              SegmentGroups = sgl
            };

            //Create Segment
            CreateSegmentRequest createSegmentRequest = new CreateSegmentRequest{
              ApplicationId = applicationId,
              WriteSegmentRequest = segment
            };

            Console.WriteLine("Creating Segment...");
            CreateSegmentResponse createSegmentResponse = client.CreateSegmentAsync(createSegmentRequest).Result;
            Console.WriteLine("Results: {0} - {1}", createSegmentResponse.HttpStatusCode, createSegmentResponse.SegmentResponse.Id);

            var newSegmentId = createSegmentResponse.SegmentResponse.Id;

            //Update Segment
            segment.Name = "GroupC-Changed";
            UpdateSegmentRequest updateSegmentRequest = new UpdateSegmentRequest{
              ApplicationId = applicationId,
              SegmentId = newSegmentId, //leave empty to create new one.
              WriteSegmentRequest = segment
            };

            Console.WriteLine("Upserting Segment...");
            UpdateSegmentResponse updateSegmentResponse = client.UpdateSegmentAsync(updateSegmentRequest).Result;
            Console.WriteLine("Results: {0}", updateSegmentResponse.HttpStatusCode);

            //GetSegment
            GetSegmentRequest segmentRequest = new GetSegmentRequest{
              ApplicationId = applicationId,
              SegmentId = newSegmentId
            };
            
            Console.WriteLine("Fetching Segment...");
            GetSegmentResponse segmentResponse = client.GetSegmentAsync(segmentRequest).Result;
            Console.WriteLine("Found: {0}", segmentResponse.SegmentResponse.Name);
        }
    }
}
