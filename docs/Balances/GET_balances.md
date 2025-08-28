### Purpose
Get all balances for user.

### Request Description

- \*\*Method\*\*: GET 
- \*\*URL\*\*: `/v1/balances`
 
### Request Parameters

#### Example Request in JSON Format

### Response Parameters

#### Successful Response
- \*\*HTTP Status Code\*\*: 200 OK

| Parameter   | Required | Data Type     | Description                            |
| ----------- | -------- | ------------- | -------------------------------------- |
| balances    | Yes      | Array 	 | List of objects all balances for user  |

Each balance entry contains the following:

| Parameter       | Required | Data Type | Description                                     |
| --------------- | -------- | --------- | ----------------------------------------------- |
| ticker          | Yes      | String    | The current currency ticker                     |
| network         | Yes      | String    | The current currency network                    |
| amount          | Yes      | Decimal   | The amount of current currency balance for user |


#### Example Successful Response

```json
{
	"balances": [
	{
	     "ticker": "ETH",
	     "network": "Etheruem",
	     "amount": 0.1
	},
	{
	     "ticker": "USDT",
	     "network": "Tectum",
	     "amount": 201.53
	}
	],
	"errorCode": "0",
	"errorMsgs": []
}```

### Error Response

#### Common Error Response Structure

| Parameter | Required | Data Type    | Description          |
| --------- | -------- | ------------ | -------------------- |
| error     | Yes      | String       | Error code           |
| message   | Yes      | String       | Error description    |
 
### Error Codes

| Error Code              | HTTP Status Code  | Error Description                  |
| ----------------------- | ----------------- | ---------------------------------- |
| 10011      		  | 400 Bad Request   | Access token is incorrect        |


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
2\. Server checks the token and returns the user's current balances
3\. If the user's Authorization Token is invalid, the server returns an error.

