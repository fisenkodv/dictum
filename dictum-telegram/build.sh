#!/usr/bin/env bash

printf "Building AWS Lambda...\n\n"
rm function.zip
npm i
zip -r function.zip
aws lambda update-function-code --function-name quote-telegram-bot --zip-file fileb://function.zip