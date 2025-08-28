# \### Purpose

# Get all balances for user.

# 

# \### Request Description

# \- \*\*Method\*\*: GET 

# \- \*\*URL\*\*: `/v1/balances`

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

# | balances    | Yes      | Array 	   | List of objects all balances for user  |



# Each balance entry contains the following:

# 

# | Parameter       | Required | Data Type | Description                                     |

# | --------------- | -------- | --------- | ----------------------------------------------- |

# | ticker          | Yes      | String    | The current currency ticker                     |

# | network         | Yes      | String    | The current currency network                    |

# | amount          | Yes      | Decimal   | The amount of current currency balance for user |





# \#### Example Successful Response

# ```json

# {

# &nbsp; "balances": \[

# &nbsp;   {

# &nbsp;     "ticker": "ETH",

# &nbsp;     "network": "Etheruem",

# &nbsp;     "amount": 0.1

# &nbsp;   },

# &nbsp;   {

# &nbsp;     "ticker": "USDT",

# &nbsp;     "network": "Tectum",

# &nbsp;     "amount": 201.53

# &nbsp;   }

# &nbsp; ],

# &nbsp; "errorCode": "0",

# &nbsp; "errorMsgs": \[]

# }```

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

# 2\. Server checks the token and returns the user's current balances

# 3\. If the user's Authorization Token is invalid, the server returns an error.

