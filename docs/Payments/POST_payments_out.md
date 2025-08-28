# \### Purpose

# Create output transaction.

# 

# \### Request Description

# \- \*\*Method\*\*: POST 

# \- \*\*URL\*\*: `/v1/payments/out`

# 

# \### Request Parameters

# | Parameter         | Required | Location | Data Type | Constraints                        | Description                        |

# | ----------------- | -------- | -------- | --------- | ---------------------------------- | ---------------------------------- |

# | network           | Yes      | Body     | String    | Current currency network (10 words)|  Current currency network          |

# | ticker            | Yes      | Body     | String    | Current currency ticker (4 words)  | Current currency ticker            |

# | amount            | Yes      | Body     | Decimal   | Positive value                     | Amount of coins to transfer        |

# | externalId        | Yes      | Body     | String    | Valid external Id (64 words)       | External identificator             |

# | requestId         | Yes      | Body     | String    | Valid request Id (64 words)        | Request identificator              |

# \#### Example Request in JSON Format

# ```json

# &nbsp;{

# &nbsp;  "network": "Ethereum",

# &nbsp;  "ticker": "Eth",

# &nbsp;  "amount": 0.012,

# &nbsp;  "externalId": "85d3c89a-2cc8-47f7-bf99-f2b21171ab1f",

# &nbsp;  "requestId": "c28aa5af-91cd-487b-95c4-2a8edcbbdaa6"

# &nbsp;}```

# \### Response Parameters

# 

# \#### Successful Response

# \- \*\*HTTP Status Code\*\*: 200 OK

# 

# | Parameter   | Required | Data Type     | Description                            |

# | ----------- | -------- | ------------- | -------------------------------------- |

# | id          | Yes      | String 	   | List of objects all active currencies  |

# | externalId  | Yes      | String 	   | List of objects all active currencies  |

# | Address     | Yes      | String 	   | List of objects all active currencies  |

# | Status      | Yes      | String 	   | List of objects all active currencies  |




# \#### Example Successful Response

# ```json

# &nbsp;{

# &nbsp;  "id": "db69811c-3c0d-48c7-bf47-afa37330af30",

# &nbsp;  "externalId": "39c32e24-5514-4826-b9bf-006b55297d15",

# &nbsp;  "address": "0x43fedf6abfd39cb6355be2952b402526e0ce4ccc",

# &nbsp;  "status": "Init",

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

# | message   | Yes      | Array        | Error description    |

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

# 2\. Server checks the token and create transaction out

# 3\. If the user's Authorization Token is invalid, the server returns an error.

