package com.javaapi;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.google.gson.JsonElement;
import com.google.gson.JsonParser;

import javax.net.ssl.SSLContext;
import javax.net.ssl.TrustManager;
import javax.net.ssl.X509TrustManager;
import java.net.URI;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.security.cert.X509Certificate;
import java.time.Duration;
import java.util.Scanner;

public class Main {

    static final String API_BASE = "https://localhost:7236/api/familia";
    static final Gson gson = new GsonBuilder().setPrettyPrinting().create();
    static HttpClient client;

    public static void main(String[] args) {
        try {
            // Honek ignoratzen du SSL-a ez dela erreala (soilik garapenean)
            TrustManager[] trustAllCerts = new TrustManager[]{
                    new X509TrustManager() {
                        public X509Certificate[] getAcceptedIssuers() { return new X509Certificate[0]; }
                        public void checkClientTrusted(X509Certificate[] certs, String authType) {}
                        public void checkServerTrusted(X509Certificate[] certs, String authType) {}
                    }
            };

            SSLContext sslContext = SSLContext.getInstance("TLS");
            sslContext.init(null, trustAllCerts, new java.security.SecureRandom());

            client = HttpClient.newBuilder()
                    .sslContext(sslContext)
                    .connectTimeout(Duration.ofSeconds(10))
                    .build();

            Scanner scanner = new Scanner(System.in);
            System.out.println("===== MENÚ =====");
            System.out.println("1. Ver familias (GET)");
            System.out.println("2. Crear nueva familia (POST)");
            System.out.print("Elige una opción (1 o 2): ");
            int opcion = scanner.nextInt();
            scanner.nextLine(); // limpiar buffer

            switch (opcion) {
                case 1 -> hacerPeticionGet();
                case 2 -> hacerPeticionPost(scanner);
                default -> System.out.println("Opción no válida.");
            }

        } catch (Exception e) {
            System.err.println("Error: " + e.getMessage());
            e.printStackTrace();
        }
    }

    public static void hacerPeticionGet() {
        try {
            HttpRequest request = HttpRequest.newBuilder()
                    .uri(URI.create(API_BASE))
                    .GET()
                    .build();

            HttpResponse<String> response = client.send(request, HttpResponse.BodyHandlers.ofString());

            System.out.println("Código de estado: " + response.statusCode());
            System.out.println("Respuesta JSON:");
            JsonElement je = JsonParser.parseString(response.body());
            String prettyJson = gson.toJson(je);
            System.out.println(prettyJson);

        } catch (Exception e) {
            System.err.println("Error al hacer GET: " + e.getMessage());
            e.printStackTrace();
        }
    }

    public static void hacerPeticionPost(Scanner scanner) {
        try {
            System.out.print("Introduce el nombre (izena) de la nueva familia: ");
            String izena = scanner.nextLine();

            // Crear JSON manualmente
            String jsonBody = gson.toJson(new Familia(0, izena));

            HttpRequest request = HttpRequest.newBuilder()
                    .uri(URI.create(API_BASE))
                    .header("Content-Type", "application/json")
                    .POST(HttpRequest.BodyPublishers.ofString(jsonBody))
                    .build();

            HttpResponse<String> response = client.send(request, HttpResponse.BodyHandlers.ofString());

            System.out.println("Código de estado: " + response.statusCode());
            System.out.println("Respuesta:");
            System.out.println(response.body());

        } catch (Exception e) {
            System.err.println("Error al hacer POST: " + e.getMessage());
            e.printStackTrace();
        }
    }

    // Clase interna para construir el JSON del POST
    static class Familia {
        int id;
        String izena;

        public Familia(int id, String izena) {
            this.id = id;
            this.izena = izena;
        }
    }
}
