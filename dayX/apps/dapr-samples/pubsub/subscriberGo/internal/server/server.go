package server

import (
	"log"
	"strconv"
	"context"
	"net/http"
	"github.com/gorilla/mux"
)

type server struct {
	port int
	router *mux.Router
	httpServer *http.Server
}

func CreateServer(port int) *server {
	s := &server{ port : port }
	s.router = mux.NewRouter()
	s.registerSubscribeToTopicsHandler().registerMyTopicHandler();
	return s
}

func (s *server) Start() error {
	log.Printf("Starting Server on port %d\n", s.port)
	port := ":"+strconv.Itoa(s.port)
	server := &http.Server{
		Addr: port,
		Handler: s.router,
	}

	s.httpServer = server

	if err := server.ListenAndServe(); err != nil {
		return err
	}

	return nil
}

func (s *server) Shutdown(ctx context.Context) *server {
	s.httpServer.Shutdown(ctx)
	return s
}