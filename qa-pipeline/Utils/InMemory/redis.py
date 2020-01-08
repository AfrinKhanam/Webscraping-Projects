from redis import Redis

class RedisClient():
    def __init__(self, host='localhost', port=6379):
        self.redis_client = Redis(host, port)

    def set_value(self, key, value, expiry_time):
        self.redis_client.set(key, value, ex=expiry_time)

        return

    def get_value(self, key):
        return self.redis_client.get(key)
