package server

import (
	"log"
	"encoding/json"
	"net/http"
)

type Message struct {
	Text string `json:Text`
}

type Payload struct {
	data Message `json:data`
}

// subscribe to topics
func (s *server) registerSubscribeToTopicsHandler() *server {
	handler := func (w http.ResponseWriter, r *http.Request) {
		topics := []string {"mytopic"}
		json,_ := json.Marshal(topics)
		w.Write(json)
		w.WriteHeader(http.StatusOK)
	}

	s.router.HandleFunc("/dapr/subscribe", handler).Methods("GET")
	return s
}

// handle message of topic 'mytopic'
func (s *server) registerMyTopicHandler() *server {
	handler := func (w http.ResponseWriter, r *http.Request) {
		decoder := json.NewDecoder(r.Body)
		
		var Palyoad struct {
			Data struct {
				Text string `json:"Text"`
			} `json:"data"`
		}

		decoder.Decode(&Palyoad)
		log.Println(Palyoad.Data.Text)
		w.WriteHeader(http.StatusOK)
	}

	s.router.HandleFunc("/mytopic", handler).Methods("POST")
	return s
}