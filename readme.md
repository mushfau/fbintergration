Repo: https://github.com/mushfau/fbintergration


# Facebook

## Server verification callback
### Method: GET
### URL: https://yourdomain.com/api/comments/facebook?hub.mode=subscribe&hub.challenge=1996213468&hub.verify_token=[developer_defined_token]
Response: [hub.challenge]


## Facebook login callback
### Method: GET
### URL: https://yourdomain.com/api/auth/facebook/callback



## Page access token generation
### Method: POST 
### URL: https://yourdomain.com/api/auth/facebook/token
Body: `{ page: [page_id], token: [user_acces_token] }`

Response: `{ access_token: [page_access_token], id: [page_id] }`



## Subscribe to application
### Method: POST
### URL: https://yourdomain.com/api/comments/subscribe	
Body: `{ page: [page_id], token: [page_access_token] }`



## Webhook notification callback
### Method: POST	
### URL: https://yourdomain.com/api/comments/facebook
Body option1 : main post
`{
    "object": "page",
    "entry": [
        {
            "id": "100473908465587",
            "time": 1603845935,
            "changes": [
                {
                    "value": {
                        "from": {
                            "id": "100473908465587",
                            "name": "Byte"
                        },
                        "message": "ABCD",
                        "post_id": "100473908465587_150691160110528",
                        "created_time": 1603845934,
                        "item": "status",
                        "published": 1,
                        "verb": "add"
                    },
                    "field": "feed"
                }
            ]
        }
    ]
}`


Body option2: comment on post
`{
    "object": "page",
    "entry": [
        {
            "id": "100473908465587",
            "time": 1603845975,
            "changes": [
                {
                    "value": {
                        "from": {
                            "id": "3844961895548656",
                            "name": "Ibrahim Mushfau Saeed"
                        },
                        "post": {
                            "status_type": "mobile_status_update",
                            "is_published": true,
                            "updated_time": "2020-10-28T00:46:10+0000",
                            "permalink_url": "https://www.facebook.com/bytemv/posts/150691160110528",
                            "promotion_status": "inactive",
                            "id": "100473908465587_150691160110528"
                        },
                        "message": "xzy",
                        "post_id": "100473908465587_150691160110528",
                        "comment_id": "150691160110528_150691260110518",
                        "created_time": 1603845970,
                        "item": "comment",
                        "parent_id": "100473908465587_150691160110528",
                        "verb": "add"
                    },
                    "field": "feed"
                }
            ]
        }
    ]
}`


 

Body option3 : reply to comment

`{
    "object": "page",
    "entry": [
        {
            "id": "100473908465587",
            "time": 1603845994,
            "changes": [
                {
                    "value": {
                        "from": {
                            "id": "3844961895548656",
                            "name": "Ibrahim Mushfau Saeed"
                        },
                        "post": {
                            "status_type": "mobile_status_update",
                            "is_published": true,
                            "updated_time": "2020-10-28T00:46:32+0000",
                            "permalink_url": "https://www.facebook.com/bytemv/posts/150691160110528",
                            "promotion_status": "inactive",
                            "id": "100473908465587_150691160110528"
                        },
                        "message": "sadfasdfdsf",
                        "post_id": "100473908465587_150691160110528",
                        "comment_id": "150691160110528_150691323443845",
                        "created_time": 1603845991,
                        "item": "comment",
                        "parent_id": "150691160110528_150691260110518",
                        "verb": "add"
                    },
                    "field": "feed"
                }
            ]
        }
    ]
}`

Response: 200 Ok





## Reply to comments
### Method: POST
### URL: https://yourdomain.com/api/comments/reply
Body: `{ commentId:[comment_id], message: [reply content], token: [page_acce_token] }`

Response: 200 Ok


 
 
# Instagram 

## Server verification callback
### Method: GET
### URL: https://yourdomain.com/api/comments/instagram?hub.mode=subscribe&hub.challenge=1996213468&hub.verify_token=[developer_defined_token]
Response: [hub.challenge]



## Webhook notification callback
### Method: POST	
### URL: https://yourdomain.com/api/comments/instagram 
Body : 
`{
    "entry": [{
      "time": "2343443535",
      "changes": [{
        "value": {
          "id": "17865799348089039",
          "text": "This is an example."
        }
      }]
    }]
  }`

