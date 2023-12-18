async function register() {
    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;

    const user = {
      username: username,
      password: password
    } 

    // Kullanıcı kayıt işlemleri burada gerçekleştirilebilir

    const response = await fetch("https://localhost:7200/api/Auth/register", {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(user)
    });
    if (response.status === 200) {
      alert('Kayıt başarılı! Username: ' + username);
    }else {
      alert('Kayıt başarısız!');
    }
  }

async function login() {
  const username = document.getElementById('loginUsername').value;
  const password = document.getElementById('loginPassword').value;

  const model = {
    username: username,
    password: password
  }

  const response = await fetch("https://localhost:7200/api/Auth/login", {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(model)
  });

  if (response.status === 200) {
    alert('Giriş başarılı! Username: ' + username);
  }else if (response.status === 400) {
    alert('Parola yanlış.');
  }else {
    alert('Giriş başarısız!');
  }
}