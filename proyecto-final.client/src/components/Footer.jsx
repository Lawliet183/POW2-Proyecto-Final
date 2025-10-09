import { styled } from 'styled-components';


function Footer({ year }) {
  return (
    <FooterContainer>
      <p>© Proyecto Encuestas - <b>Copyright (c) {year} Francisco Jarquin</b></p>
    </FooterContainer>
  );
}


const FooterContainer = styled.footer`
  all: unset;
  box-sizing: border-box;
  display: block;
  position: fixed;
  bottom: 0;
  left: 0;
  width: 100%;
  margin: 0;
  padding: 2px;
  background-color: #333;
  color: white;
  text-align: center;
  z-index: 1000;
`;


export default Footer;