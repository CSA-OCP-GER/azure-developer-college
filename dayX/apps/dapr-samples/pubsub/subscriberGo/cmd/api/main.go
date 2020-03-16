package main

import (
	"context"
	"flag"
	"log"
	"os"
	"os/signal"
	"subscribergo/internal/server"
	"time"
)

func main() {
	numberPtr := flag.Int("port", 5000, "Server port")
	port := *numberPtr
	server := server.CreateServer(port)

	go func() {
		if err := server.Start(); err != nil {
			log.Fatalln(err)
		}
	}()

	c := make(chan os.Signal, 1)
	// We'll accept graceful shutdowns when quit via SIGINT (Ctrl+C)
	// SIGKILL, SIGQUIT or SIGTERM (Ctrl+/) will not be caught.
	signal.Notify(c, os.Interrupt)

	// Block until we receive our signal.
	<-c

	// Create a deadline to wait for.
	ctx, cancel := context.WithTimeout(context.Background(), 500*time.Millisecond)
	defer cancel()
	// Doesn't block if no connections, but will otherwise wait
	// until the timeout deadline.
	go func() {
		server.Shutdown(ctx)
	}()

	// wait until server is shutdown or timeout is reached
	<-ctx.Done()

	log.Println("shutting down")
	os.Exit(0)
}
