using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using InfusionGames.CityScramble.Models;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace InfusionGames.CityScramble.Services
{
    public class DataService : IDataService
    {
        private readonly IMobileServiceClient _client;

        public DataService(IMobileServiceClient client)
        {
            _client = client;
        }

        public async Task<Profile> GetProfileAsync()
        {
            throw new NotImplementedException("GetProfileAsync");
        }

        public async Task<Team> JoinTeamAsync(string teamCode)
        {
            throw new NotImplementedException("JoinTeamAsync");
        }

        public async Task<IEnumerable<Race>> GetRacesAsync()
        {
            try
            {
                var dummy = new Race { Name = "Dummy Race" };

                return new[] { dummy };

                //var races = await _client.InvokeApiAsync<IEnumerable<Race>>("race", HttpMethod.Get, null);

                //return races;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving races.", ex);
            }
        }

        public async Task<Race> GetRaceAsync(string id)
        {
            throw new NotImplementedException("GetRaceAsync");
        }

        public async Task<IEnumerable<TeamClue>> GetCluesForTeamAsync(string raceId)
        {
            throw new NotImplementedException("GetCluesForTeamAsync");
        }

        public async Task<ClueResponse> GetClueResponse(string clueId)
        {
            throw new NotImplementedException("GetClueResponse");
        }

        public async Task<ClueResponse> PostClueResponse(string clueId, double lat, double lng, byte[] dataArray, byte[] version)
        {
            PutClueResponse responsePlaceholder = null;
            Uri uploadedImage = null;

            // 1) issue PUT to obtain place holder
            try
            {
                var putParameters = new Dictionary<string, string>();
                putParameters["clueId"] = clueId;

                var putRequestBody = new PutClueRequest
                {
                    MediaType = "jpg",
                    Version = version
                };

                responsePlaceholder = await _client.InvokeApiAsync<PutClueRequest, PutClueResponse>("clueresponse/{clueId}", putRequestBody, HttpMethod.Put, putParameters);
                if (responsePlaceholder == null)
                {
                    // invalid response object - guard
                    throw new Exception("Placeholder was null.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to create placeholder for clue response.", ex);
            }

            try
            {
                // 2) Upload image to Storage Container
                // Get the URI generated that contains the SAS 
                // and extract the storage credentials.
                StorageCredentials cred = new StorageCredentials(responsePlaceholder.SasQueryString);
                uploadedImage = new Uri(responsePlaceholder.EndpointUri);

                // Instantiate a Blob store container based on the info in the returned item.
                CloudBlobContainer container = new CloudBlobContainer(
                    new Uri(string.Format("https://{0}/{1}",
                        uploadedImage.Host, responsePlaceholder.ContainerName)), cred);

                // Upload the new image as a BLOB from the stream.
                CloudBlockBlob blobFromSASCredential = container.GetBlockBlobReference(responsePlaceholder.ResourceName);
                blobFromSASCredential.Properties.ContentType = "image/jpeg";
                //await blobFromSASCredential.SetPropertiesAsync();
                //await blobFromSASCredential.UploadFromStreamAsync(data);
                await blobFromSASCredential.UploadFromByteArrayAsync(dataArray, 0, dataArray.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Unable to upload image to Blob Storage at URL {0}.", uploadedImage.ToString()), ex);
            }

            // 3) Inform server that image is uploaded
            try
            {
                var postRequestBody = new PostClueRequest
                {
                    Data = uploadedImage.ToString(),
                    Latitude = lat,
                    Longitude = lng,
                    Version = responsePlaceholder.Version
                };

                var postParameters = new Dictionary<string, string>();
                postParameters["clueId"] = clueId;

                var response = await _client.InvokeApiAsync<PostClueRequest, ClueResponse>("clueresponse/{clueId}", postRequestBody, HttpMethod.Post, postParameters);

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("Error submitting clue response details.", ex);
            }
        }

        public async Task<bool> PostLocationUpdate(Location location)
        {
            throw new NotImplementedException("PostLocationUpdate");
        }

        public async Task<string> RegisterForPushNotifications(string regId)
        {
            throw new NotImplementedException("RegisterForPushNotifications");
        }

        public async Task<bool> UpdateDeviceInfoForPushNotifications(string regId, DeviceRegistration device)
        {
            throw new NotImplementedException("UpdateDeviceInfoForPushNotifications");
        }

        public async Task<bool> DeletePushRegistration(string regId)
        {
            throw new NotImplementedException("DeletePushRegistration");
        }

    }
}
