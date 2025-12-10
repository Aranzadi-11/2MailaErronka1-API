<?php
$baseURL = "https://localhost:7236"; // Reemplázalo si usas otro puerto
?>

<!DOCTYPE html>
<html lang="eu">
<head>
  <meta charset="UTF-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1.0" />
  <title>Familien Zerrenda</title>
  <script src="https://cdn.tailwindcss.com"></script>
  <script src="https://code.jquery.com/jquery-3.7.1.min.js"></script>
</head>
<body class="bg-gray-100 p-6">
  <div class="max-w-xl mx-auto bg-white shadow-xl rounded-xl p-6">
    <h1 class="text-2xl font-bold mb-4 text-center">Familien Zerrenda</h1>

    <form id="familiaForm" class="flex gap-2 mb-6">
      <input id="izenaInput" type="text" placeholder="Sartu familia izena"
             class="flex-grow p-2 border rounded" required />
      <button type="submit" class="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600">
        Gehitu
      </button>
    </form>

    <ul id="familiaList" class="space-y-2">
      <!-- Familiek hemen agertuko dira -->
    </ul>
  </div>

  <script>
    const baseURL = '<?= $baseURL ?>';

    function kargatuFamiliak() {
      $.ajax({
        url: baseURL + '/api/familia',
        method: 'GET',
        success: function (data) {
          $('#familiaList').empty();
          if (data.length === 0) {
            $('#familiaList').append('<li class="text-gray-500">Ez dago familirik.</li>');
            return;
          }

          data.forEach(function (familia) {
            $('#familiaList').append(
              `<li class="p-2 border rounded bg-gray-50">#${familia.id} - ${familia.izena}</li>`
            );
          });
        },
        error: function () {
          alert('Errorea familiak eskuratzean.');
        }
      });
    }

    $('#familiaForm').on('submit', function (e) {
      e.preventDefault();
      const izena = $('#izenaInput').val().trim();
      if (!izena) return;

      $.ajax({
        url: baseURL + '/api/familia',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ izena: izena }),
        success: function () {
          $('#izenaInput').val('');
          kargatuFamiliak(); // Eguneratu zerrenda
        },
        error: function () {
          alert('Errorea familia gordetzean.');
        }
      });
    });

    // Hasierako karga
    $(document).ready(function () {
      kargatuFamiliak();
    });
  </script>
</body>
</html>
