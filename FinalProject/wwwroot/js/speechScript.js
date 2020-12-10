onload = function() {
    if ('speechSynthesis' in window) with(speechSynthesis) {

        const activeButton = 'btn btn-dark';
        const inactiveButton = 'btn btn-outline-dark';
        const playButton = document.querySelector('#play');
        const pauseButton = document.querySelector('#pause');
        let flag = false;

        playButton.addEventListener('click', onClickPlay);
        pauseButton.addEventListener('click', onClickPause);

        function onClickPlay() {
            if(!flag){
                flag = true;
                let utterance = new SpeechSynthesisUtterance(document.querySelector('#wiki').textContent);

                utterance.voice = getVoices()[0];
                utterance.onend = function(){
                    flag = false;
                };

                playButton.className = activeButton;

                speak(utterance);
            }
             if (paused) { /* unpause/resume narration */
                playButton.className = activeButton;
                pauseButton.className = inactiveButton;
                resume();
            } 
        }

        function onClickPause() {
            if(speaking && !paused){ /* pause narration */
                pauseButton.className = activeButton;
                playButton.className = inactiveButton;
                pause();
            }
        }
    }

    else { /* speech synthesis not supported */
        let controls = document.querySelector('#AudioControls');
        controls.className = 'hidden';
    }

}
