# \### Purpose

# Get information about current transaction.

# 

# \### Request Description

# \- \*\*Method\*\*: GET 

# \- \*\*URL\*\*: `/v1/payments/{transactionId}`

# 

# \### Request Parameters

# 

# \#### Example Request in JSON Format

# 

# \### Response Parameters

# 

# \#### Successful Response

# \- \*\*HTTP Status Code\*\*: 200 OK

# 

# | Parameter   | Required | Data Type     | Description                                 |

# | ----------- | -------- | ------------- | ------------------------------------------- |

# | transaction | Yes      | Object 	   | Objects with information about transaction  |



# Each transactions entry contains the following:

# 

# | Parameter       | Required | Data Type | Description                                                                     |

# | --------------- | -------- | --------- | ------------------------------------------------------------------------------- |

# | id              | Yes      | String    | Transaction identificator                                                       |

# | amount          | Yes      | Decimal   | Transaction amount                                                              |

# | ticker          | Yes      | String    | Transaction currency ticker                                                     |

# | network         | Yes      | String    | Transaction currency network                                                    |

# | createdAt       | Yes      | DateTime  | Timestamp when transaction was created                                          |

# | externalId      | Yes      | String    | External transaction ID (from payment processor or blockchain)                  |

# | type            | Yes      | String    | Transaction type: Common (regular) or Commission (fee transaction)              |

# | status          | Yes      | String    | Transaction status                                                              |

# | addressFrom     | Yes      | String    | Sender address (blockchain address)                                             |

# | addressTo       | Yes      | String    | Recipient address (blockchain address)                                          |

# | feeAmount       | Yes      | Decimal   | Fee amount for the transaction                                                  |

# | feeTicker       | Yes      | String    | Currency of the fee ticker                                                      |

# | direction       | Yes      | String    | Transaction direction: In (deposit) or Out (withdrawal)                         |

# | groupId         | Yes      | String    | Group ID for transactions that belong together (like multi-step payments)       |

# | hash            | Yes      | String    | Blockchain transaction hash                                                     |

# | errorCode       | Yes      | String    | Error code if transaction failed                                                |

# | errorMsg        | Yes      | String    | Error message if transaction failed                                             |




# \#### Example Successful Response

# ```json

# &nbsp;{

# &nbsp;  "transactions": 

# &nbsp;    {

# &nbsp;      "id": "00000000-0000-0000-0000-000000000000",

# &nbsp;      "amount": 0.01,

# &nbsp;      "ticker": "BNB",

# &nbsp;      "network": "Binance",

# &nbsp;      "createdAt": "0001-01-01T00:00:00",

# &nbsp;      "externalId": "219efe76-4614-4efd-9f93-58e8ca300167",

# &nbsp;      "type": "Common",

# &nbsp;      "status": "Completed",

# &nbsp;      "addressFrom": null,

# &nbsp;      "addressTo": null,

# &nbsp;      "feeAmount": null,

# &nbsp;      "feeTicker": null,

# &nbsp;      "direction": "In",

# &nbsp;      "groupId": null,

# &nbsp;      "hash": "0x1234",

# &nbsp;      "errorCode": null,

# &nbsp;      "errorMsg": null

# &nbsp;    },

# &nbsp;  "errorCode": 0,

# &nbsp;  "errorMsgs": []

# &nbsp;}```

# 

# \### Error Response

# \#### Common Error Response Structure

# 

# | Parameter | Required | Data Type    | Description          |

# | --------- | -------- | ------------ | -------------------- |

# | error     | Yes      | String       | Error code           |

# | message   | Yes      | String       | Error description    |

# 

# \### Error Codes

# 

# | Error Code              | HTTP Status Code  | Error Description                  |

# | ----------------------- | ----------------- | ---------------------------------- |

# | 10011      		    | 400 Bad Request   | Access token is incorrect          |

# 

# \#### Example Error Response

# ```json

# {

# &nbsp; "errorCode": "10011",

# &nbsp; "errorMsgs": \[

# &nbsp;   "Access token is incorrect"

# &nbsp; ]

# }

# ```

# \### Workflow

# 1\. User sends a request. With the user's Authorization Token

# 2\. Server checks the token ant transaction id and returns the transaction information

# 3\. If the user's Authorization Token is invalid, the server returns an error.

