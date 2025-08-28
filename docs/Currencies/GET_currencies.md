# \### Purpose

# Get all active currencies.

# 

# \### Request Description

# \- \*\*Method\*\*: GET 

# \- \*\*URL\*\*: `/v1/currencies`

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

# | Parameter   | Required | Data Type     | Description                            |

# | ----------- | -------- | ------------- | -------------------------------------- |

# | currencies  | Yes      | Array 	   | List of objects all active currencies  |



# Each currency entry contains the following:

# 

# | Parameter       | Required | Data Type | Description                                     |

# | --------------- | -------- | --------- | ----------------------------------------------- |

# | ticker          | Yes      | String    | The current currency ticker                     |

# | network         | Yes      | String    | The current currency network                    |

# | currencyKey     | Yes      | String    | The current currency key in our system          |





# \#### Example Successful Response

# ```json

# &nbsp;{

# &nbsp;  "currencies": [

# &nbsp;    {

# &nbsp;      "currencyKey": "eth-ethereum-erc20",

# &nbsp;     "ticker": "ETH",

# &nbsp;      "network": "Ethereum"

# &nbsp;    },

# &nbsp;    {

# &nbsp;      "currencyKey": "eth-ethereum-erc20",

# &nbsp;      "ticker": "ETH",

# &nbsp;      "network": "Ethereum"

# &nbsp;    }

# &nbsp;  ],

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

# 2\. Server checks the token and returns the all active currencies

# 3\. If the user's Authorization Token is invalid, the server returns an error.

