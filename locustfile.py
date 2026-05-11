from locust import HttpUser, task, between

class FleetLogicUser(HttpUser):
    # Кожен "водій" робить запити з паузою від 1 до 3 секунд
    wait_time = between(1, 3)

    @task(2)
    def load_telemetry(self):
        # Імітуємо оновлення карти кожні 3 секунди
        self.client.get("/api/Telemetries")

    @task(1)
    def load_drivers(self):
        # Імітуємо завантаження таблиці водіїв
        self.client.get("/api/Drivers")
        self.client.get("/api/Users")