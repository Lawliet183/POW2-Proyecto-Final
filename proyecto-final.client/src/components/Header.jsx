import { styled } from 'styled-components';

function Header({ title }) {
  return (
    <HeaderContainer>
      <StyledHeaderText>{title}</StyledHeaderText>

      <nav>
        <NavItemsContainer>
          <NavItem><a href="#home">Home</a></NavItem>
          <NavItem><a href="#about">About</a></NavItem>
          <NavItem><a href="#contact">Contact</a></NavItem>
        </NavItemsContainer>
      </nav>
    </HeaderContainer>
  );
}


const HeaderContainer = styled.header`
  all: unset;
  box-sizing: border-box;
  display: block;
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  margin: 0;
  padding: 1rem;
  background-color: #333;
  color: white;
  text-align: center;
  z-index: 1000;
`;

const NavItemsContainer = styled.ul`
  list-style-type: none;
  margin: 0;
  padding: 0;
`;

const NavItem = styled.li`
  display: inline;
  margin: 0 15px;
`;

const StyledHeaderText = styled.h2`
  margin: 0 0 0.7rem 0;
`;


export default Header;