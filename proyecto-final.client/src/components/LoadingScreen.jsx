import { keyframes, styled } from 'styled-components';

import loadingIcon from '@/assets/loader.svg';


function LoadingScreen() {
  return (
    <Loader>
      <img src={loadingIcon} />
    </Loader>
  );
}


const spin = keyframes`
  from {
    transform: rotate(0deg);
  }
  to {
    transform: rotate(360deg);
  }
`;

const Loader = styled.div`
  height: 5rem;
  width: 5rem;
  margin: auto;
  animation: ${spin} 2s infinite linear;
`;


export default LoadingScreen;