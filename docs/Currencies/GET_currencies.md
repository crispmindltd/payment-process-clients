### Purpose
Get all active currencies.

### Request Description
- \*\*Method\*\*: GET 
- \*\*URL\*\*: `/v1/currencies`

### Request Parameters

#### Example Request in JSON Format

### Response Parameters

#### Successful Response
- \*\*HTTP Status Code\*\*: 200 OK

| Parameter   | Required | Data Type     | Description                            |
| ----------- | -------- | ------------- | -------------------------------------- |
| currencies  | Yes      | Array 	 | List of objects all active currencies  |

Each currency entry contains the following:

| Parameter       | Required | Data Type | Description                                     |
| --------------- | -------- | --------- | ----------------------------------------------- |
| ticker          | Yes      | String    | The current currency ticker                     |
| network         | Yes      | String    | The current currency network                    |
| currencyKey     | Yes      | String    | The current currency key in our system          |


#### Example Successful Response

```json

{
  "currencies": [
     {
	"currencyKey": "eth-ethereum-erc20",
	"ticker": "ETH",
	"network": "Ethereum"
     },
     {
	"currencyKey": "eth-ethereum-erc20",
	"ticker": "ETH",
	"network": "Ethereum"
     }
  ],
  "errorCode": 0,
  "errorMsgs": []
}
```

### Error Response

#### Common Error Response Structure

| Parameter | Required | Data Type    | Description          |
| --------- | -------- | ------------ | -------------------- |
| error     | Yes      | String       | Error code           |
| message   | Yes      | String       | Error description    |


### Error Codes

| Error Code              | HTTP Status Code  | Error Description                  |
| ----------------------- | ----------------- | ---------------------------------- |
| 10011      		  | 400 Bad Request   | Access token is incorrect          |


#### Example Error Response

```json
{
  "errorCode": "10011",
  "errorMsgs": [
     "Access token is incorrect"
  ]
}
 ```

### Workflow

1\. User sends a request. With the user's Authorization Token

2\. Server checks the token and returns the all active currencies

3\. If the user's Authorization Token is invalid, the server returns an error.

