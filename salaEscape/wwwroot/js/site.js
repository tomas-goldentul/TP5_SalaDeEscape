﻿// Funciones del temporizador
function formatTime(minutes, seconds) {
    return `${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`;
}

function updateTimer() {
    const timerElement = document.getElementById('timer');
    if (!timerElement) return;

    let endTime = localStorage.getItem('escapeRoomEndTime');
    if (!endTime) {
        // Si no hay tiempo guardado, inicializar con 40 minutos
        endTime = Date.now() + (40 * 60 * 1000);
        localStorage.setItem('escapeRoomEndTime', endTime);
    }

    const timeLeft = Math.max(0, endTime - Date.now());
    const minutes = Math.floor(timeLeft / 60000);
    const seconds = Math.floor((timeLeft % 60000) / 1000);

    timerElement.textContent = formatTime(minutes, seconds);

    // Agregar clase warning cuando queden 5 minutos o menos
    if (minutes <= 5) {
        timerElement.classList.add('warning');
    } else {
        timerElement.classList.remove('warning');
    }

    // Si el tiempo se acabó
    if (timeLeft <= 0) {
        timerElement.textContent = '00:00';
        timerElement.classList.add('warning');
        alert('¡Se acabó el tiempo!');
        window.location.href = '/Home/Derrota';
        return;
    }

    setTimeout(updateTimer, 1000);
}

// Iniciar el temporizador cuando se carga el documento
document.addEventListener("DOMContentLoaded", () => {
    // Si estamos en la página de inicio o créditos, reiniciar el temporizador
    if (window.location.pathname === '/' || 
        window.location.pathname === '/Home' || 
        window.location.pathname === '/Home/Index' ||
        window.location.pathname === '/Home/Creditos' ||
        window.location.pathname === '/Home/Historia') {
        localStorage.removeItem('escapeRoomEndTime');
    }
    
    // Si estamos en la página de derrota o victoria, no mostrar el temporizador
    if (window.location.pathname === '/Home/Derrota' || 
        window.location.pathname === '/Home/SalaFinal') {
        const timerElement = document.getElementById('timer');
        if (timerElement) {
            timerElement.style.display = 'none';
        }
        return;
    }

    updateTimer();

    //Sala 3
    const sala3 = document.getElementById("sala3");
    if (sala3) {
        const mensaje = document.getElementById("mensaje");
        const display = document.getElementById("pad-display");
        const numKeys = document.querySelectorAll(".num-key");
        const clearButton = document.getElementById("clear");
        const submitButton = document.getElementById("submit");
        const padbtn = document.getElementById("padbtn");
        if (display && numKeys.length > 0) {
            numKeys.forEach((key) => {
                key.addEventListener("click", () => {
                    if (display.value.length < 5) {
                        display.value += key.getAttribute("data-value");
                    }
                });
            });

            clearButton?.addEventListener("click", () => {
                display.value = "";
            });

            padbtn?.addEventListener("click", () => {
                document.getElementById("pad").style.display = "block";
            });

            submitButton?.addEventListener("click", () => {
                const inputValue = display.value;
                if (inputValue && inputValue.length === 5) {
                    const form = document.createElement("form");
                    form.method = "POST";
                    form.action = "/Home/Sala3";
                    
                    const input = document.createElement("input");
                    input.type = "hidden";
                    input.name = "clave";
                    input.value = inputValue;
                    
                    form.appendChild(input);
                    document.body.appendChild(form);
                    form.submit();
                } else {
                    alert('Por favor, ingrese un número válido de 5 dígitos');
                }
            });
        }
        if (qteTerminado === true) {
            document.getElementById("qte").style.display = "none";
            sala3.style.backgroundImage = "url('/images/fondoSala34.png')";
            document.getElementById("padbtn").style.display = "inline-block";

        }
        const fases = [
            { tecla: "w", imagen: "/images/fondoSala30.png", mensaje: "Spammea", tiempoLimite: 3000 },
            { tecla: "s", imagen: "/images/fondoSala31.png", mensaje: "Spammea", tiempoLimite: 3500 },
            { tecla: "a", imagen: "/images/fondoSala32.png", mensaje: "Spammea", tiempoLimite: 4000 },
            { tecla: "d", imagen: "/images/fondoSala33.png", mensaje: "Spammea", tiempoLimite: 4500 }
        ];

        const teclas = {
            w: document.getElementById("teclaW"),
            s: document.getElementById("teclaS"),
            a: document.getElementById("teclaA"),
            d: document.getElementById("teclaD")
        };

        let faseActual = 0;
        let pulsaciones = 0;
        let faseEnCurso = false;
        let tiempoInicio = 0;
        let temporizador = null;
        const pulsacionesRequeridas = 15;
        const pausaEntreFases = 2000;

        function destacarTecla(tecla) {
            Object.values(teclas).forEach(el => el.classList.remove("destacar"));
            if (teclas[tecla]) {
                teclas[tecla].classList.add("destacar");
            }
        }

        function iniciarFase() {
            if (faseActual < fases.length) {
                mensaje.textContent = "Preparando siguiente fase...";
                faseEnCurso = false;
                destacarTecla(null);

                setTimeout(() => {
                    pulsaciones = 0;
                    faseEnCurso = true;
                    tiempoInicio = Date.now();

                    const fase = fases[faseActual];
                    sala3.style.backgroundImage = `url('${fase.imagen}')`;
                    destacarTecla(fase.tecla);

                    clearTimeout(temporizador);
                    temporizador = setTimeout(() => {
                        if (faseEnCurso) {
                            faseEnCurso = false;
                            if (pulsaciones < pulsacionesRequeridas) {
                                mensaje.textContent = "¡Fallaste! Inténtalo de nuevo.";
                                destacarTecla(null);
                                faseActual = 0;
                                setTimeout(iniciarFase, pausaEntreFases);
                            }
                        }
                    }, fase.tiempoLimite);
                }, pausaEntreFases);
            }
        }

        document.addEventListener("keydown", (event) => {
            if (!faseEnCurso) return;

            const fase = fases[faseActual];
            const teclaPresionada = event.key.toLowerCase();

            if (teclaPresionada == fase.tecla) {
                pulsaciones++;
                mensaje.textContent = `${fase.mensaje}`;

                if (pulsaciones >= pulsacionesRequeridas) {
                    faseEnCurso = false;
                    clearTimeout(temporizador);

                    if (faseActual === fases.length - 1) {
                        setTimeout(() => {
                            sala3.style.backgroundImage = "url('/images/fondoSala34.png')";
                            destacarTecla(null);
                            alert("42987");
                            document.getElementById("padbtn").style.display = "inline-block";
                            fetch('/Home/CompletarQte', { method: 'POST' })
                                .then(() => window.location.reload());
                        }, 50);
                    } else {
                        faseActual++;
                        destacarTecla(null);
                        iniciarFase();
                    }
                }
            }else{
                faseEnCurso = false;
                clearTimeout(temporizador);
                mensaje.textContent = "¡Tecla incorrecta! Inténtalo de nuevo.";
                destacarTecla(null);
                faseActual = 0;
                setTimeout(iniciarFase, pausaEntreFases);
            }
        });

        iniciarFase();
    }

    // Sala 4
  const sala4 = document.querySelector('.sala4');
    if (sala4) {
        console.log('Inicializando Sala 4...');
        
        const areaJuego = sala4.querySelector('.area-juego');
        const bate = sala4.querySelector('.bate');
    
        if (!areaJuego || !bate) {
            console.error('No se encontraron elementos necesarios para la Sala 4');
            return;
        }
    
        const CODIGO = '40647';
        const NUM_RATAS = 10;
        let ratasGolpeadas = 0;
        let digitoActual = 0;
        let animacionEnProgreso = false;
    
        function crearRatas() {
            console.log('Creando ratas...');
            for (let i = 0; i < NUM_RATAS; i++) {
                const rata = document.createElement('div');
                rata.className = 'rata';
                posicionarRataAleatoriamente(rata);
                // 1 de cada 2 ratas mostrará código
                rata.dataset.muestraCodigo = (i % 2 === 0).toString();
                rata.addEventListener('click', (e) => golpearRata(e, rata));
                areaJuego.appendChild(rata);
                console.log('Rata creada:', i + 1);
    
                setInterval(() => {
                    if (!rata.classList.contains('golpeada')) {
                        posicionarRataAleatoriamente(rata);
                    }
                }, Math.random() * 900 + 900);
            }
        }
    
        function posicionarRataAleatoriamente(rata) {
            const maxX = areaJuego.clientWidth - 60;
            const maxY = areaJuego.clientHeight - 60;
            const x = Math.random() * maxX;
            const y = Math.random() * maxY;
            
            rata.style.left = x + 'px';
            rata.style.top = y + 'px';
        }
    
        function golpearRata(evento, rata) {
            if (rata.classList.contains('golpeada') || animacionEnProgreso) return;
    
            console.log('Golpeando rata...');
            animacionEnProgreso = true;
            
            const rect = areaJuego.getBoundingClientRect();
            const x = evento.clientX - rect.left;
            const y = evento.clientY - rect.top;
    
            bate.style.left = (x - 40) + 'px';
            bate.style.top = (y - 160) + 'px';
            bate.style.display = 'block';
            bate.style.transform = 'rotate(-45deg)';
    
            setTimeout(() => {
                bate.style.transform = 'rotate(0deg)';
                rata.classList.add('golpeada');
    
                if (rata.dataset.muestraCodigo === 'true' && digitoActual < CODIGO.length) {
                    const numero = document.createElement('div');
                    numero.className = 'numero';
                    numero.textContent = CODIGO[digitoActual];
                    numero.style.left = (x - 15) + 'px';
                    numero.style.top = (y - 15) + 'px';
                    areaJuego.appendChild(numero);
    
                    setTimeout(() => {
                        numero.remove();
                    }, 1000);
    
                    digitoActual++;
                }
    
                setTimeout(() => {
                    bate.style.display = 'none';
                }, 1000);
    
                ratasGolpeadas++;
                animacionEnProgreso = false;
    
                console.log('Ratas golpeadas:', ratasGolpeadas);
                if (digitoActual === CODIGO.length) {
                    setTimeout(() => {
                        mostrarFormularioClave(CODIGO);
                    }, 1000);
                }
            }, 200);
        }
        setTimeout(crearRatas, 500);
    }
});

function mostrarPista(id) {
    document.getElementById(id).style.display = "flex";
}

function cerrarPista(id) {
    document.getElementById(id).style.display = "none";
}
