#!/usr/bin/env bash
# wait-for-it.sh: wait for a host and TCP port to become available

set -e

host="$1"
port="$2"
shift 2

echo "Waiting for $host:$port..."

while ! nc -z "$host" "$port"; do
  sleep 1
done

echo "$host:$port is available"
exec "$@"
