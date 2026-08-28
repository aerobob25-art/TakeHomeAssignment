package main

import (
	"encoding/json"
	"flag"
	"log"
	"math/rand/v2"
	"net/http"
	"strconv"
	"sync"
	"sync/atomic"
	"time"
)

func main() {
	addr := flag.String("addr", "localhost:8080", "address at which to listen for traffic")
	flag.Parse()

	srv := &server{
		mu:            new(sync.Mutex),
		users:         make(map[int]struct{}),
		registerCalls: new(atomic.Int32),
		loginCalls:    new(atomic.Int32),
	}
	http.HandleFunc("POST /register", srv.register)
	http.HandleFunc("POST /login", srv.login)
	log.Printf("Listening on %s", *addr)
	log.Fatal(http.ListenAndServe(*addr, nil))
}

type server struct {
	mu    *sync.Mutex
	users map[int]struct{}

	registerCalls *atomic.Int32
	loginCalls    *atomic.Int32
}

type registerResp struct {
	UserID int `json:"user_id"`
}

func (srv *server) register(w http.ResponseWriter, r *http.Request) {
	if r.FormValue("user_id") != "" {
		http.Error(w, "did you mean to call /login?", http.StatusBadRequest)
		return
	}
	registerCalls := srv.registerCalls.Add(1)
	switch registerCalls % 4 {
	case 1:
		http.Error(w, "failed to register!", http.StatusInternalServerError)
	case 2:
		w.Write([]byte(`{"user_id": "not an int"}`))
	case 3:
		hang(r)
	default:
		json.NewEncoder(w).Encode(registerResp{UserID: srv.newID()})
	}
}

func (srv *server) newID() int {
	srv.mu.Lock()
	defer srv.mu.Unlock()

	for {
		id := rand.Int()
		if _, ok := srv.users[id]; !ok {
			srv.users[id] = struct{}{}
			return id
		}
	}
}

func (srv *server) haveID(id int) bool {
	srv.mu.Lock()
	defer srv.mu.Unlock()

	_, ok := srv.users[id]
	return ok
}

func (srv *server) forget(id int) {
	srv.mu.Lock()
	defer srv.mu.Unlock()
	delete(srv.users, id)
}

func (srv *server) login(w http.ResponseWriter, r *http.Request) {
	id, err := strconv.Atoi(r.FormValue("user_id"))
	if err != nil {
		http.Error(w, "missing or invalid user_id", http.StatusBadRequest)
		return
	}
	if !srv.haveID(id) {
		http.Error(w, "user not found", http.StatusNotFound)
		return
	}

	loginCalls := srv.loginCalls.Add(1)
	switch loginCalls % 10 {
	case 1:
		http.Error(w, "failed to login!", http.StatusInternalServerError)
	case 2:
		hang(r)
	case 3:
		// oops, forgot the user exists or it got deleted
		srv.forget(id)
		http.Error(w, "user not found", http.StatusNotFound)
		return
	default:
		log.Printf("successful login from user %v", id)
		w.Write([]byte("success!"))
	}
}

func hang(r *http.Request) {
	t := time.NewTimer(time.Hour)
	defer t.Stop()
	select {
	case <-r.Context().Done():
	case <-t.C:
	}
}
