FROM golang:alpine AS build

RUN apk update && apk add make

# Create appuser.
ENV USER=appuser
ENV UID=10001 

RUN adduser \    
    --disabled-password \    
    --gecos "" \    
    --home "/nonexistent" \    
    --shell "/sbin/nologin" \    
    --no-create-home \    
    --uid "${UID}" \    
    "${USER}"

WORKDIR /app
COPY . ./

RUN make build
RUN mv ./dist/linux_amd64/release/api ./

FROM scratch AS runtime

# Import the user and group files from the builder.
COPY --from=build /etc/passwd /etc/passwd
COPY --from=build /etc/group /etc/group

WORKDIR /app
COPY --from=build /app/api .

USER appuser:appuser

ENTRYPOINT [ "./api", "-port", "5000" ]