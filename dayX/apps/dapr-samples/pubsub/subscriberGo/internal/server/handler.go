package server

import (
	"encoding/json"
	"log"
	"net/http"
)

// type topic struct {
// 	Name                  string `json:"name"`
// 	MaxConcurrentMessages int    `json:"maxConcurrentMessages"`
// }

// subscribe to topics
func (s *server) registerSubscribeToTopicsHandler() *server {
	handler := func(w http.ResponseWriter, r *http.Request) {
		topics := []string{"mytopic"}
		json, _ := json.Marshal(topics)

		// topics := []topic{topic{Name: "mytopic", MaxConcurrentMessages: 10}}
		// json, _ := json.Marshal(topics)
		log.Printf("Subscribed to: %s", string(json))
		w.Write(json)
		w.WriteHeader(http.StatusOK)
	}

	s.router.HandleFunc("/dapr/subscribe", handler).Methods("GET")
	return s
}

// handle message of topic 'mytopic'
func (s *server) registerMyTopicHandler() *server {
	handler := func(w http.ResponseWriter, r *http.Request) {
		decoder := json.NewDecoder(r.Body)

		var palyoad struct {
			Data struct {
				Text string `json:"Text"`
			} `json:"data"`
		}

		decoder.Decode(&palyoad)
		log.Println(palyoad.Data.Text)
		w.WriteHeader(http.StatusOK)
	}

	s.router.HandleFunc("/mytopic", handler).Methods("POST")
	return s
}
