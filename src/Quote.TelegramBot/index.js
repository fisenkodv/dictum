const fetch = require('node-fetch');
const { random, includes } = require('lodash');
const api = `https://api.fisenko.net/quotes`;

async function getData(url) {
  try {
    const data = await fetch(url);
    const json = await data.json();
    const quote = json.text;
    const author = json.author;
    return `<b>${quote}</b>\n\u2014 <i>${author}</i>`;
  } catch (err) {
    console.error('Fail to fetch data: ' + err);
    return 'Try one more time.';
  }
}

function getTrigger(str) {
  const triggerWords = ['though', 'though', 'quote', 'dictum', 'wise'];
  for (let item of triggerWords) {
    if (includes(str, item.toLowerCase())) {
      return true;
    }
  }
  return false;
}

exports.handler = async event => {
  const body = JSON.parse(event.body);
  const text = body.message.text;

  const userMsg = text.toLowerCase();

  let botMsg;
  if (getTrigger(userMsg)) {
    botMsg = await getData(api);
  }

  const msg = {
    method: 'sendMessage',
    parse_mode: 'HTML',
    chat_id: body.message.chat.id,
    text: botMsg,
    reply_markup: JSON.stringify({
      keyboard: [[{ text: 'Get Quote' }]]
    })
  };

  const response = {
    statusCode: 200,
    headers: {
      'Content-Type': 'application/json; charset=utf-8'
    },
    body: JSON.stringify(msg),
    isBase64Encoded: false
  };
  return response;
};
