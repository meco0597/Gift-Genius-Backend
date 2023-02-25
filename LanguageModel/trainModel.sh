#!/bin/sh
openai -k $OPEN_API_KEY api fine_tunes.create -t $1 -m davinci