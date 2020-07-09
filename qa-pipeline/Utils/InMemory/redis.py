from redis import Redis
from configparser import ConfigParser
config_file_path = '../../config.ini'
config = ConfigParser()
config.read(config_file_path)

class RedisClient():
    host = config.get('redis_credentials', 'host')
    port = config.getint('redis_credentials', 'port')
    def __init__(self, host=host, port=port):
        self.redis_client = Redis(host, port)

    def set_value(self, key, value, expiry_time):
        self.redis_client.set(key, value, ex=expiry_time)

        return

    def get_value(self, key):
        return self.redis_client.get(key)
