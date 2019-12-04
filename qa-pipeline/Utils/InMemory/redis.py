from redis import Redis

class RedisClient():
    def __init__(self, host='localhost', port=6379):
        self.redis_client = Redis(host, port)

    def set_value(self, key, value):
        self.redis_client.set(key, value)

        return

    def get_value(self, key):
        return self.redis_client.get(key)
