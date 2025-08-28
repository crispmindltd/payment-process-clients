### Purpose
Get information about current transaction.
 
### Request Description
- \*\*Method\*\*: GET 
- \*\*URL\*\*: `/v1/payments/{transactionId}`

### Request Parameters

#### Example Request in JSON Format

### Response Parameters

#### Successful Response
- \*\*HTTP Status Code\*\*: 200 OK
 
| Parameter   | Required | Data Type     | Description                                 |
| ----------- | -------- | ------------- | ------------------------------------------- |
| transaction | Yes      | Object 	 | Objects with information about transaction  |


Each transactions entry contains the following:

| Parameter       | Required | Data Type | Description                                                                     |
| --------------- | -------- | --------- | ------------------------------------------------------------------------------- |
| id              | Yes      | String    | Transaction identificator                                                       |
| amount          | Yes      | Decimal   | Transaction amount                                                              |
| ticker          | Yes      | String    | Transaction currency ticker                                                     |
| network         | Yes      | String    | Transaction currency network                                                    |
| createdAt       | Yes      | DateTime  | Timestamp when transaction was created                                          |
| externalId      | Yes      | String    | External transaction ID (from payment processor or blockchain)                  |
| type            | Yes      | String    | Transaction type: Common (regular) or Commission (fee transaction)              |
| status          | Yes      | String    | Transaction status                                                              |
| addressFrom     | Yes      | String    | Sender address (blockchain address)                                             |
| addressTo       | Yes      | String    | Recipient address (blockchain address)                                          |
| feeAmount       | Yes      | Decimal   | Fee amount for the transaction                                                  |
| feeTicker       | Yes      | String    | Currency of the fee ticker                                                      |
| direction       | Yes      | String    | Transaction direction: In (deposit) or Out (withdrawal)                         |
| groupId         | Yes      | String    | Group ID for transactions that belong together (like multi-step payments)       |
| hash            | Yes      | String    | Blockchain transaction hash                                                     |
| errorCode       | Yes      | String    | Error code if transaction failed                                                |
| errorMsg        | Yes      | String    | Error message if transaction failed                                             |


#### Example Successful Response

```json
{
  "transaction": {
    "id": "00000000-0000-0000-0000-000000000000",
    "amount": 0.01,
    "ticker": "BTC",
    "network": "Binance",
    "createdAt": "0001-01-01T00:00:00",
    "externalId": "31d29d02-35c5-4879-8cd4-4678ae6ec860",
    "type": "Common",
    "status": "Completed",
    "addressFrom": null,
    "addressTo": null,
    "feeAmount": null,
    "feeTicker": null,
    "direction": "In",
    "groupId": null,
    "hash": "0x1234",
    "errorCode": null,
    "errorMsg": null
  },
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
  "errorCode": 0,
  "errorMsgs": [
    "string"
  ]
}
```

### Workflow

1\. User sends a request. With the user's Authorization Token

2\. Server checks the token ant transaction id and returns the transaction information

3\. If the user's Authorization Token is invalid, the server returns an error.

