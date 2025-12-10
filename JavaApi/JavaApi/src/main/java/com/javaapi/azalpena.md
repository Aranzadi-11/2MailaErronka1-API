🔐 ¿Por qué falla HTTPS normalmente?

Java es muy estricto con los certificados. Cuando haces una conexión a https://localhost, Java espera que:

El certificado esté emitido por una autoridad de certificación (CA) válida.

El nombre del dominio (localhost) esté en el CN (Common Name) del certificado.

Pero cuando estás desarrollando con un servidor local (como .NET o Spring Boot), se suele usar un certificado autofirmado, y por tanto, Java rechaza la conexión con este error:

javax.net.ssl.SSLHandshakeException: PKIX path building failed

✅ ¿Cómo se soluciona en el código?

Con esta parte que agregamos al principio de tu main():

TrustManager[] trustAllCerts = new TrustManager[]{
new X509TrustManager() {
public X509Certificate[] getAcceptedIssuers() { return new X509Certificate[0]; }
public void checkClientTrusted(X509Certificate[] certs, String authType) {}
public void checkServerTrusted(X509Certificate[] certs, String authType) {}
}
};

SSLContext sslContext = SSLContext.getInstance("TLS");
sslContext.init(null, trustAllCerts, new java.security.SecureRandom());

HttpClient client = HttpClient.newBuilder()
.sslContext(sslContext)
.connectTimeout(Duration.ofSeconds(10))
.build();

🧠 ¿Qué hace esto exactamente?

Creamos un TrustManager personalizado que no realiza ninguna verificación del certificado del servidor.

Creamos un SSLContext con ese TrustManager.

Construimos un HttpClient usando ese SSLContext.

Resultado: Java no se queja del certificado inseguro ni del hostname localhost.

⚠️ ¡Muy importante!

Esto solo debes usarlo en desarrollo o pruebas locales. Nunca hagas esto en una aplicación de producción porque:

Acepta cualquier certificado, aunque sea falso.

Permite ataques de "man-in-the-middle" si estás en una red insegura.

✅ Alternativas más seguras (para producción)

Si tu aplicación va a producción, deberías:

Usar un certificado real emitido por una CA (por ejemplo, con Let's Encrypt).

O bien importar el certificado autofirmado en el almacén de claves de Java (cacerts).