### Purpose
Confirm one transaction and start waiting.
 
### Request Description
- \*\*Method\*\*: POST
- \*\*URL\*\*: `/v1/transaction/{transactionId}/confirm`

### Request Parameters

#### Example Request in JSON Format

### Response Parameters

#### Successful Response
- \*\*HTTP Status Code\*\*: 200 OK

| Parameter   | Required | Data Type     | Description                            |
| ----------- | -------- | ------------- | -------------------------------------- |
| id	      | Yes      | String 	 | Transaction identificator              |
| status      | Yes      | String 	 | Transaction status                     |


#### Example Successful Response

```json
{
  "id": "00000000-0000-0000-0000-000000000000",
  "status": "Init",
  "errorCode": 0,
  "errorMsgs": []
}
```

### Error Response

#### Common Error Response Structure

| Parameter | Required | Data Type    | Description          |
| --------- | -------- | ------------ | -------------------- |
| error     | Yes      | String       | Error code           |
| message   | Yes      | Array        | Error description    |

### Error Codes

| Error Code              | HTTP Status Code  | Error Description                  |
| ----------------------- | ----------------- | ---------------------------------- |
| 10011      		  | 400 Bad Request   | Access token is incorrect          |

#### Example Error Response

```json
{
  "errorCode": "0",
  "errorMsgs": [
    "string"
  ]
}
```

### Workflow

1\. User sends a request. With the user's Authorization Token and transaction id

2\. Server checks the token and transaction id and returns the transaction status

3\. If the user's Authorization Token is invalid, the server returns an error.

