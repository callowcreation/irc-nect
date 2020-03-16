IRCnect
=======================

Connect to IRC network servers to send and receive chat messages.

<!-- TOC -->

- [Motivation](#motivation)
- [See](#see)
- [Installation](#installation)
- [Loading and configuration](#loading-and-configuration)
- [Common Usage](#common-usage)
- [Contact](#contact)

<!-- /TOC -->

## Motivation

Creating a Unity Asset to allow Unity developers to make interactive projects that allow viewers in a Twitch chat room to manipulate the game/application.

## See
- RFC 1459 - [Internet Relay Chat Protocol](https://tools.ietf.org/html/rfc1459.html)

- Twitch - [Chatbots & IRC Guide](https://dev.twitch.tv/docs/irc/guide)

## Installation
```sh
C# Solusion - Download and include in your peoject
```

## Loading and configuration
A Node server is required, express is used here.

```js
const TwitchOAuth = require('twitch-oauth');

const state = 'a-Unique-ID-98765432-For_Security';

const twitchOAuth = new TwitchOAuth({
    client_id: process.env.CLIENT_ID,
    client_secret: process.env.CLIENT_SECRET,
    redirect_uri: process.env.REDIRECT_URI,
    scopes: [
        'user:edit:broadcast'
    ]
}, state);

const express = require('express');
const app = express();

app.get('/authorize', (req, res) => {
    res.redirect(twitchOAuth.authorizeUrl);
});

// redirect_uri ends up here
app.get('/auth-callback', (req, res) => {
    const qs = require('querystring');
    const req_data = qs.parse(req.url.split('?')[1]);
    const code = req_data['code'];
    const state = req_data['state'];

    if (twitchOAuth.confirmState(state) === true) {
        twitchOAuth.fetchToken(code).then(json => {
            if (json.success === true) {
                console.log('authenticated');
                res.redirect('/home');
            } else {
                res.redirect('/failed');
            }
        }).catch(err => console.error(err));
    } else {
        res.redirect('/failed');
    }
});

```

## Common Usage

```js
app.get('/user', (req, res) => {
    const url = `https://api.twitch.tv/helix/users/extensions?user_id=101223367`;
    twitchOAuth.getEndpoint(url)
        .then(json => res.status(200).json(json));
});
```

#### Handling exceptions

```js
twitchOAuth.getEndpoint(`https://api.twitch.tv/helix/users/extensions?user_id=101223367`)
    .then(json => console.log("User Data", json))
    .catch(err => console.error(err));
```

## Contact
- [Contact caLLowCreation](http://callowcreation.com/home/contact-us/)
- [https://www.twitch.tv/callowcreation](https://www.twitch.tv/callowcreation)
- [https://twitter.com/callowcreation](https://twitter.com/callowcreation)

## License

MIT