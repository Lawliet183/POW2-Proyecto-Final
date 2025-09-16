import loadingIcon from '@/assets/loader.svg';

import '@/components/LoadingScreen.css';


function LoadingScreen() {
  return (
    <div className="loader">
      <img src={loadingIcon} />
    </div>
  );
}

export default LoadingScreen;